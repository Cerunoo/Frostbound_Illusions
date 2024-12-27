using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [Space(4)]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;

    private IPlayerController _player;

    private void Awake()
    {
        _player = GetComponentInParent<IPlayerController>();
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

    private void OnJumped(bool doubleJump)
    {
        if (TimelineController.Instance.isPlay) return;

        source.PlayOneShot(jumpSound);
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        if (TimelineController.Instance.isPlay) return;

        if (grounded)
        {
            source.PlayOneShot(landingSound);
        }
    }
}