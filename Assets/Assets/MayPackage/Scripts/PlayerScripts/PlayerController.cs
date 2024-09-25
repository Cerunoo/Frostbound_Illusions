using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace MayPackage
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float runSpeed = 9f;
        public float airWalkSpeed = 4f;
        public float jumpImpulse = 10f;

        public float coyotTime = 0.2f;
        public float coyotTimeCounter;

        public float jumpBufferTime = 0.2f;
        public float jumpBufferCounter;

        public float fallMult;
        public float maxFallSpeed = 18f;
        public float baseGravity;

        Vector2 moveInput = Vector2.zero;
        TouchingDirections touchingDirections;

        public bool _isAlive = true;

        Rigidbody2D rb;
        Animator animator;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();
        }

        private void Update()
        {
            FasterFall();
            CanJump();
            Jump();

            if (_isAlive)
            {
                rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);

                animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
            }
        }

        private void FixedUpdate()
        {

        }

        public float currentMoveSpeed
        {
            get
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        if (IsRunning)
                        {
                            return (runSpeed + airWalkSpeed) / 1.75f;
                        }
                        else
                        {
                            return (walkSpeed + airWalkSpeed) / 1.75f;
                        }
                    }
                }
                else
                {
                    return 0.1f;
                }
            }
        }

        private void Jump()
        {
            if (_isAlive)
            {
                if (coyotTimeCounter > 0f && jumpBufferCounter > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);

                    jumpBufferCounter = 0f;
                }

                if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

                    coyotTimeCounter = 0f;
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    jumpBufferCounter = jumpBufferTime;
                }
            }
        }

        private void CanJump()
        {
            if (touchingDirections.IsGrounded)
            {
                coyotTimeCounter = coyotTime;
            }
            else
            {
                if (coyotTimeCounter > 0)
                {
                    coyotTimeCounter -= Time.deltaTime;
                }
            }

            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        private void FasterFall()
        {
            if (rb.velocity.y < 0f)
            {
                rb.gravityScale = baseGravity * fallMult;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
            else
            {
                rb.gravityScale = baseGravity;
            }
        }

        [SerializeField]
        private bool _isMoving = false;

        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            private set
            {
                _isMoving = value;
                animator.SetBool(AnimationStrings.isMoving, value);
            }
        }

        [SerializeField]
        private bool _isRunning = false;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                animator.SetBool(AnimationStrings.isRunning, value);
            }
        }

        [SerializeField] private bool _isFacingRight = true;

        public bool IsFacingRight
        {
            get { return _isFacingRight; }
            private set
            {
                if (_isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }

                _isFacingRight = value;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (_isAlive)
            {
                IsMoving = moveInput != Vector2.zero;

                SetFacingDirection(moveInput);
            }
            else
            {
                IsMoving = false;
            }

        }

        private void SetFacingDirection(Vector2 moveInput)
        {
            if (_isAlive)
            {
                if (moveInput.x > 0 && !IsFacingRight)
                {
                    IsFacingRight = true;
                }
                else if (moveInput.x < 0 && IsFacingRight)
                {
                    IsFacingRight = false;
                }
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsRunning = true;
            }
            else if (context.canceled)
            {
                IsRunning = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // ����� ��� ��� ������, �� ��-�� ������� � ��� ���������.
        }
    }

}