using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    private Animator animator;
    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0f;

    public UnityEvent OnMoved;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input.x != 0f || input.y != 0f)
            animator.SetBool("Walk", true);
        else
            animator.SetBool("Walk", false);

        moveDir = new Vector3(input.x, 0, input.y);
        OnMoved?.Invoke();
    }

    private void Jump()
    {
        if (isGround() && ySpeed < 0f)
            ySpeed = -1f;
        else
            ySpeed += Physics.gravity.y * Time.deltaTime;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if(isGround())
            ySpeed = jumpSpeed;
    }

    private bool isGround()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1f, 0.5f, Vector3.down, out hit, 0.51f, LayerMask.GetMask("Enviroment"));
    }
}
