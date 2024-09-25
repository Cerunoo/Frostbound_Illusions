using Cinemachine;
using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MayPackage;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerDeath : MonoBehaviour
{
    public Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Scene activeScene;

    [SerializeField] private CinemachineVirtualCamera deathCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    private PlayerController playerController;
    public CameraShake cameraShake;

    [SerializeField] private float respawnTime = 1f;

    public float kbForce;
    public bool knockFromRight;

    private Animator animator;

    public Animator flashOnDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerController._isAlive = false;
            animator.SetTrigger(AnimationStrings.TriggerDeath);
        }
    }


    public void Die()
    {
        cameraShake.ShakeCamera();
        deathCamera.transform.position = targetCamera.transform.position;
        Destroy(targetCamera);
        if (knockFromRight == true)
        {
            rb.velocity = new Vector2(-2, kbForce);
        }
        else
        {
            rb.velocity = new Vector2(2, kbForce);
        }
        cc.isTrigger = true;
        StartCoroutine(Respawn(respawnTime));

        flashOnDeath.SetTrigger(AnimationStrings.DeathFlash);
    }

    IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(activeScene.name);
    }
}