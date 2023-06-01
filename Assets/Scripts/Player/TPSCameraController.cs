using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] Transform aimTarget;
    [SerializeField] LayerMask layer;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float lookDistance;
    [SerializeField] int zoomMagnification;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;
    private bool OnLookArounded;
    private bool isZooming;
    private Quaternion originalRotation;

    private Vector3 initRootPos;

    Vector3 lookPoint;

    private void Start()
    {
        initRootPos = cameraRoot.localPosition;
    }

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
            aimTarget.position = lookPoint;
            lookPoint.y = transform.position.y;
            transform.LookAt(lookPoint);
        }
    }

    private void OnZoom(InputValue value)
    {
        if (value.isPressed)
        {
            Zoom();
        }
        else
        {
            isZooming = false;
            cameraRoot.localPosition = initRootPos;
        }
    }

    private void Zoom()
    {
        if (!isZooming)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraRoot.position, cameraRoot.forward, out hit, zoomMagnification, layer))
            {
                cameraRoot.position = hit.point + (hit.normal * 0.5f);
            }
            else
            {
                cameraRoot.position += cameraRoot.forward * zoomMagnification;
                //cameraRoot.localPosition = initRootPos + (Vector3.forward * zoomMagnification);
            }
        }
        isZooming = true;
    }
}
