using System;
using System.Collections;
using UnityEngine;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        [SerializeField] private bool facingRight;

        private InputController controls;

        public PlayerStamina stamina;

        #region Interface

        public float FrameInput => _frameInput.Horizontal;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

            controls = new InputController();

            controls.Move.Jump.performed += context =>
            {
                _jumpToConsume = true;
                _frameInput.JumpHeld = true;
                _timeJumpWasPressed = _time;
            };
            controls.Move.Jump.canceled += context => _frameInput.JumpHeld = false;

            controls.Move.Dash.performed += context => { if (!isDashing) _dashToConsume = true; };
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpHeld = _frameInput.JumpHeld,
                Horizontal = controls.Move.Move.ReadValue<float>()
            };

            if (_stats.SnapInput) _frameInput.Horizontal = Mathf.Abs(_frameInput.Horizontal) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Horizontal);
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleDash();
            HandleGravity();

            ApplyMovement();
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            Vector2 playerScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * playerScale, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * playerScale, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                doubleJump = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;
        private bool doubleJump;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (_jumpToConsume && !_grounded && !doubleJump && !isDashing && stamina.TrySpendStamina())
            {
                doubleJump = true;
                ExecuteJump(true);
            }

            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote && !isDashing) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump(bool doubleJump = false)
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = (!doubleJump) ? _stats.JumpPower : _stats.DoubleJumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Horizontal == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Horizontal * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }

            if (_frameInput.Horizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (_frameInput.Horizontal < 0 && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        #endregion

        #region Dash

        private bool _dashToConsume;
        private float dashDelay;
        private bool isDashing;
        private bool canNewDash;

        private void HandleDash()
        {
            dashDelay -= Time.deltaTime;
            if (_grounded) canNewDash = true;

            if (_dashToConsume && dashDelay < 0 && !isDashing && canNewDash && stamina.TrySpendStamina()) StartCoroutine(ExecuteDash());
            _dashToConsume = false;
        }

        private IEnumerator ExecuteDash()
        {
            dashDelay = _stats.DashDelay;
            isDashing = true;
            canNewDash = false;

            float elapsedTime = 0f;
            while (elapsedTime < _stats.DashTime)
            {
                float velocityMultiplier = _stats.DashPower * _stats.DashPowerCurve.Evaluate(elapsedTime);
                _frameVelocity.x = velocityMultiplier * (facingRight ? 1 : -1);

                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            _frameVelocity.x = _frameInput.Horizontal * _stats.MaxSpeed;
            isDashing = false;
            yield break;
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                if (isDashing) _frameVelocity.y = 0;
                else _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpHeld;
        public float Horizontal;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public float FrameInput { get; }
    }
}