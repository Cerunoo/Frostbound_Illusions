using UnityEngine;

namespace FollusionController
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxTilt = 5;
        [SerializeField] private float _tiltSpeed = 20;
        [SerializeField] private LayerMask DetectGroundLayer;

        [Header("Particles")]
        [SerializeField] private ParticleSystem _jumpParticles;
        [SerializeField] private ParticleSystem _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles;
        [SerializeField] private ParticleSystem _landParticles;
        [SerializeField] private ParticleSystem _doubleJumpParticles;

        // [Header("Audio Clips"), SerializeField]
        // private AudioClip[] _footsteps;

        private Animator _anim;
        // private AudioSource _source;
        private IPlayerController _player;
        private bool _grounded;
        private ParticleSystem.MinMaxGradient _currentGradient;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            // _source = GetComponent<AudioSource>();
            _player = GetComponentInParent<IPlayerController>();
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
            _player.Dashed += OnDashed;
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
            _player.Dashed -= OnDashed;
        }

        private void Update()
        {
            DetectGroundColor();
            HandleMove();
            HandleCharacterTilt();
            HandleFall();
        }

        private void HandleMove()
        {
            var inputStrength = Mathf.Abs(_player.FrameDirection.x);
            if (_player.IsRunning) _anim.SetBool(isRunningKey, inputStrength > 0);
            else _anim.SetBool(isWalkingKey, inputStrength > 0);
            _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * inputStrength, 2 * Time.deltaTime);
        }

        private void HandleCharacterTilt()
        {
            var runningTilt = _grounded ? Quaternion.Euler(0, 0, _maxTilt * _player.FrameDirection.x) : Quaternion.identity;
            _anim.transform.up = Vector3.RotateTowards(_anim.transform.up, runningTilt * Vector2.up, _tiltSpeed * Time.deltaTime, 0f);
        }

        private void HandleFall()
        {
            _anim.SetBool(FallKey, _player.FrameDirection.y < 0);
        }

        private void OnJumped(bool doubleJump)
        {
            _anim.SetTrigger(JumpKey);
            _anim.ResetTrigger(GroundedKey);

            if (_grounded) // Avoid coyote
            {
                SetColor(_jumpParticles);
                SetColor(_launchParticles);
                _jumpParticles.Play();
            }
            if (doubleJump)
            {
                SetColor(_doubleJumpParticles);
                _doubleJumpParticles.Play();
            }
        }

        private void OnDashed()
        {
            _anim.SetTrigger(DashKey);
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;

            if (grounded)
            {
                DetectGroundColor();
                SetColor(_landParticles);

                _anim.SetTrigger(GroundedKey);
                // _source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
                _moveParticles.Play();

                _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
                _landParticles.Play();
            }
            else
            {
                _moveParticles.Stop();
            }
        }

        private void DetectGroundColor()
        {
            var hit = Physics2D.Raycast(transform.position, Vector3.down, 2, DetectGroundLayer);

            if (!hit || hit.collider.isTrigger || !hit.transform.TryGetComponent(out SpriteRenderer r)) return;
            var color = r.color;
            _currentGradient = new ParticleSystem.MinMaxGradient(color * 0.9f, color * 1.2f);
            SetColor(_moveParticles);
        }

        private void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        private static readonly int isRunningKey = Animator.StringToHash("isRunning");
        private static readonly int isWalkingKey = Animator.StringToHash("isWalking");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int FallKey = Animator.StringToHash("Fall");
        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int DashKey = Animator.StringToHash("Dash");
    }
}