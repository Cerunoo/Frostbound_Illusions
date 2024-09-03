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
            HandleRun();
        }

        private void HandleRun()
        {
            var inputStrength = Mathf.Abs(_player.FrameInput);
            _anim.SetBool(isRunningKey, inputStrength > 0);
            _anim.SetFloat("Test", _player.FrameInput);
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
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
    }
}