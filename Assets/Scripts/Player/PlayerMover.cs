using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;

    private CharacterController controller;
    private Vector3 moveDir;
    private Animator animator;

    private float ySpeed = 0f;
    private float lastSpeed;
    private bool isJumpping;
    private bool isWalking;
    private bool isGround;

    private void Awake()
    {
        lastSpeed = 0f;
        isWalking = true;
        isJumpping = false;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + Vector3.up * 1f, 0.5f, Vector3.down, out hit, 0.5001f, LayerMask.GetMask("Enviroment")))
            isGround = true;
        else
            isGround = false;

        if (isGround && isJumpping)
        {
            animator.SetBool("Jump", false);
            isJumpping = false;
        }
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Move()
    {
        if (moveDir.magnitude == 0)
            lastSpeed = Mathf.Lerp(lastSpeed, 0, 0.5f);
        else if (isWalking)
            lastSpeed = Mathf.Lerp(lastSpeed, walkSpeed, 0.5f);
        else
            lastSpeed = Mathf.Lerp(lastSpeed, runSpeed, 0.5f);

        controller.Move(transform.forward * moveDir.z * lastSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * lastSpeed * Time.deltaTime);

        animator.SetFloat("ySpeed", moveDir.z, 0.1f, Time.deltaTime); // z : ySpeed
        animator.SetFloat("xSpeed", moveDir.x, 0.1f, Time.deltaTime); // x : xSpeed
        animator.SetFloat("Speed", lastSpeed);
    }

    private void OnJump(InputValue value)
    {
        if (isGround)
        {
            ySpeed = jumpForce;

            animator.SetBool("Jump", true);
            isJumpping = true;
        }
    }

    private void Jump()
    {
        if (isGround && ySpeed < 0f)
            ySpeed = 0f;
        else
            ySpeed += Physics.gravity.y * Time.deltaTime;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
        {
            lastSpeed = runSpeed;
            isWalking = false;
        }
        else
        {
            lastSpeed = walkSpeed;
            isWalking = true;
        }
    }
}
