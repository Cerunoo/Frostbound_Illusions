using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _maxTilt = 5;
    [SerializeField] private float _tiltSpeed = 20;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _jumpParticles;
    [SerializeField] private ParticleSystem _launchParticles;
    [SerializeField] private ParticleSystem _moveParticles;
    [SerializeField] private ParticleSystem _landParticles;
    [SerializeField] private ParticleSystem _doubleJumpParticles;
    [SerializeField] private ParticleSystem _dashParticles;
    [SerializeField] private ParticleSystem _dashRingParticles;

    // [Header("Audio Clips"), SerializeField]
    // private AudioClip[] _footsteps;

    private Animator _anim;
    // private AudioSource _source;
    private IPlayerController _player;
    private bool _grounded;

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
        _player.DashedChanged += OnDashedChanged;
    }

    private void OnDisable()
    {
        _player.Jumped -= OnJumped;
        _player.GroundedChanged -= OnGroundedChanged;
        _player.DashedChanged -= OnDashedChanged;
    }

    private void Update()
    {
        HandleMove();
        HandleCharacterTilt();
        HandleFall();
    }

    private void HandleMove()
    {
        var inputStrength = Mathf.Abs(_player.FrameDirection.x);
        _anim.SetBool(isRunningKey, !_player.IsSticky ? (_player.IsRunning ? inputStrength > 0 : false) : false);
        _anim.SetBool(isWalkingKey, !_player.IsSticky ? (!_player.IsRunning ? inputStrength > 0 : false) : true);
        _anim.SetFloat(StickyDividerKey, _player.IsSticky ? 1 / _player.StickyDivider : 1);
        _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * inputStrength, 2 * Time.deltaTime);
    }

    private void HandleCharacterTilt()
    {
        var runningTilt = _grounded ? Quaternion.Euler(0, 0, _maxTilt * _player.FrameDirection.x) : Quaternion.identity;
        _anim.transform.up = Vector3.RotateTowards(_anim.transform.up, runningTilt * Vector2.up, _tiltSpeed * Time.deltaTime, 0f);
    }

    private void HandleFall()
    {
        _anim.SetBool(FallKey, _player.FrameDirection.y < -1.5f); // -1.5f условно 0, так как на земле directionY = -1.5f
    }

    private void OnJumped(bool doubleJump)
    {
        _anim.SetTrigger(JumpKey);
        _anim.ResetTrigger(GroundedKey);

        if (_grounded) // Avoid coyote
        {
            _jumpParticles.Play();
        }
        else if (doubleJump)
        {
            _doubleJumpParticles.Play();
        }
    }

    private bool moveParticlesOldStatePlaying;
    private void OnDashedChanged(bool dashed)
    {
        if (dashed)
        {
            _anim.SetTrigger(DashKey);

            _dashParticles.Play();
            _dashRingParticles.Play();
            moveParticlesOldStatePlaying = _moveParticles.isPlaying;
            _moveParticles.Stop();
        }
        else
        {
            if (moveParticlesOldStatePlaying) _moveParticles.Play();
            _moveParticles.Play();
            _dashParticles.Stop();
        }
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        _grounded = grounded;

        if (grounded)
        {
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

    private static readonly int isRunningKey = Animator.StringToHash("isRunning");
    private static readonly int isWalkingKey = Animator.StringToHash("isWalking");
    private static readonly int JumpKey = Animator.StringToHash("Jump");
    private static readonly int FallKey = Animator.StringToHash("Fall");
    private static readonly int GroundedKey = Animator.StringToHash("Grounded");
    private static readonly int DashKey = Animator.StringToHash("Dash");
    private static readonly int StickyDividerKey = Animator.StringToHash("StickyDivider");
}