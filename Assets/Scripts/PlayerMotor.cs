using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    Camera cam;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    private void FixedUpdate()
    {
        if (PauseMenu.isPaused)
            return;

        PerformMovement();
        PerformRotation();
        PerformThrust();
    }

    private void PerformThrust()
    {
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    public void PerformMovement()
    {
        if (velocity != Vector3.zero){
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    public void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam!=null)
        {
            currCameraRotationX -= cameraRotationX;
            currCameraRotationX = Mathf.Clamp(currCameraRotationX, -90f, 90f);

            cam.transform.localEulerAngles = new Vector3(currCameraRotationX, 0, 0);
        }
    }

    internal void applythruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
}
