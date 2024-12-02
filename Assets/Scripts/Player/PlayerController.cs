using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private ScriptableStats _stats;
    [SerializeField] private bool useStamina;
    private PlayerStamina stamina;

    [Header("Debug")]
    public bool disableMove;
    public bool isRunning;
    public bool isSticky;
    public bool facingRight;

    private FrameInput _frameInput;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    private Vector2 _frameVelocity;
    private bool _cachedQueryStartInColliders;

    #region Interface

    public Vector2 FrameDirection => new Vector2(_frameInput.Horizontal, _frameVelocity.y);
    public bool IsRunning => isRunning;
    public bool IsSticky => isSticky;
    public float StickyDivider => _stats.StickyDivider;
    public event Action<bool, float> GroundedChanged;
    public event Action<bool> Jumped;
    public event Action<bool> DashedChanged;

    #endregion

    private float _time;

    private void Awake()
    {
        Instance = this;

        if (useStamina) stamina = FindFirstObjectByType<PlayerStamina>();

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Player.Jump.performed += context =>
            {
                if (!disableMove)
                {
                    _jumpToConsume = true;
                    _frameInput.JumpHeld = true;
                    _timeJumpWasPressed = _time;
                }
            };
            InputController.Instance.controls.Player.Dash.performed += context => { if (!disableMove && !isDashing) _dashToConsume = true; };
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    private void GatherInput()
    {
        float inputHorizontal = InputController.Instance ? InputController.Instance.controls.Player.Horizontal.ReadValue<float>() : 0;
        _frameInput = new FrameInput
        {
            JumpHeld = !disableMove ? Convert.ToBoolean(InputController.Instance ? InputController.Instance.controls.Player.Jump.ReadValue<float>() : 0) : false,
            Horizontal = !disableMove ? inputHorizontal != 0 ? inputHorizontal : Mathf.Lerp(_frameInput.Horizontal, inputHorizontal, Time.fixedDeltaTime * 10f) : 0,
        };

        if (_stats.SnapInput) _frameInput.Horizontal = Mathf.Abs(_frameInput.Horizontal) < _stats.HorizontalDeadZoneThreshold ? 0 : _frameInput.Horizontal;
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
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * playerScale, _col.direction, 0, Vector2.down, _stats.GrounderDistance, _stats.GroundLayer);
        bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size * playerScale, _col.direction, 0, Vector2.up, _stats.GrounderDistance, _stats.GroundLayer);

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
        if (_jumpToConsume && !_grounded && !doubleJump && !isDashing && useStamina && stamina.TrySpendStamina())
        {
            doubleJump = true;
            ExecuteJump(true);
        }

        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote && !isDashing) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump(bool doubleJump = false)
    {
        float jumpPower = !doubleJump ? (isSticky ? _stats.JumpPower / _stats.StickyDivider : _stats.JumpPower) : _stats.DoubleJumpPower;

        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = jumpPower;
        Jumped?.Invoke(doubleJump);
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
            float speed = isSticky ? (_stats.MaxWalkSpeed / _stats.StickyDivider) : (isRunning ? _stats.MaxRunSpeed : _stats.MaxWalkSpeed);
            if (!_grounded) speed *= _stats.airMultiplier;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Horizontal * speed, _stats.Acceleration * Time.fixedDeltaTime);
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
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
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

        if (_dashToConsume && dashDelay < 0 && !isDashing && canNewDash && useStamina && stamina.TrySpendStamina()) StartCoroutine(ExecuteDash());
        _dashToConsume = false;
    }

    private IEnumerator ExecuteDash()
    {
        dashDelay = _stats.DashDelay;
        isDashing = true;
        canNewDash = false;
        DashedChanged?.Invoke(isDashing);

        float elapsedTime = 0f;
        while (elapsedTime < _stats.DashTime)
        {
            float velocityMultiplier = _stats.DashPower * _stats.DashPowerCurve.Evaluate(elapsedTime);
            _frameVelocity.x = velocityMultiplier * (facingRight ? 1 : -1);

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _frameVelocity.x = _frameInput.Horizontal * (isRunning ? _stats.MaxRunSpeed : _stats.MaxWalkSpeed);
        isDashing = false;
        DashedChanged?.Invoke(isDashing);
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

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

    #region SetFunc rbStates

    public void SetSticky(bool value)
    {
        isSticky = value;
    }

    // ...

    #endregion
}

public struct FrameInput
{
    public bool JumpHeld;
    public float Horizontal;
}

public interface IPlayerController
{
    public Vector2 FrameDirection { get; }
    public bool IsRunning { get; }
    public bool IsSticky { get; }
    public float StickyDivider { get; }

    public event Action<bool, float> GroundedChanged;
    public event Action<bool> Jumped;
    public event Action<bool> DashedChanged;
}