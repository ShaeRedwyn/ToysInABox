using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float cameraDistance;
    public float rotateSpeed;
    public float smoothSpeed;

    private Camera mainCamera;
    private Vector3 targetEulerRotation;
    private Vector2 previousMousePosition;
    private bool inverseControl;

    void Start()
    {
        targetEulerRotation = transform.rotation.eulerAngles;
        mainCamera = Camera.main;
        mainCamera.transform.localPosition = new Vector3(cameraDistance, 0, 0);
        mainCamera.transform.LookAt(transform.position, Vector3.up);
    }

    void Update()
    {
        GetMouseMovement();
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 currentRotation = Vector3.Lerp(transform.rotation.eulerAngles, targetEulerRotation, smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetEulerRotation), smoothSpeed);
    }

    private void GetMouseMovement()
    {
        if(Input.GetMouseButton(0))
        {
            if(Input.GetMouseButtonDown(0))
            {
                previousMousePosition = Input.mousePosition;
            }
            Vector2 mouseMovement = (Vector2)Input.mousePosition - previousMousePosition;
            mouseMovement *= rotateSpeed;

            if(inverseControl)
            {
                mouseMovement.x = - mouseMovement.x;
            }

            targetEulerRotation.y += mouseMovement.x;
            targetEulerRotation.z += -mouseMovement.y;

            previousMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            inverseControl = (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 180) || (transform.rotation.eulerAngles.z < 270 && transform.rotation.eulerAngles.z > 180);
        }
    }
}
