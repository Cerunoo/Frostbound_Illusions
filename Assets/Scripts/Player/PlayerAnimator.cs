using UnityEngine;

namespace TarodevController
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")]
        private Animator _anim;
        private IPlayerController _player;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _player = GetComponent<IPlayerController>();
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
        }

        private void Update()
        {
            HandleMove();
            HandleFall();
        }

        private void HandleMove()
        {
            var inputStrength = Mathf.Abs(_player.FrameDirection.x);
            if (_player.IsRunning) _anim.SetBool(isRunningKey, inputStrength > 0);
            else _anim.SetBool(isWalkingKey, inputStrength > 0);
        }

        private void HandleFall()
        {
            _anim.SetBool(FallKey, _player.FrameDirection.y < 0);
        }

        private void OnJumped()
        {
            _anim.SetTrigger(JumpKey);
            _anim.ResetTrigger(GroundedKey);
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            if (grounded)
            {
                _anim.SetTrigger(GroundedKey);
            }
        }

        private static readonly int isRunningKey = Animator.StringToHash("isRunning");
        private static readonly int isWalkingKey = Animator.StringToHash("isWalking");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int FallKey = Animator.StringToHash("Fall");
        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
    }
}