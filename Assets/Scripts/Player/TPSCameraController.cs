using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float lookDistance;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;
    private bool OnLookArounded;
    private Quaternion originalRotation;

    Vector3 lookPoint;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

    private void Update()
    {
        Rotate();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRotation += -lookDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (!OnLookArounded)
        {
            cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        else
        {
            cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            transform.rotation = originalRotation;
        }
    }

    private void OnLookAround(InputValue value)
    {
        if (value.isPressed)
        {
            OnLookArounded = true;
            originalRotation = transform.rotation;
        }
        else
        {
            OnLookArounded = false;
        }
    }

    private void Rotate()
    {
        if (!OnLookArounded)
        {
            lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
            lookPoint.y = transform.position.y;
            transform.LookAt(lookPoint);
        }
    }
}
