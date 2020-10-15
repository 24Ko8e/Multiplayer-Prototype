using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

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

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
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
            cam.transform.Rotate(-cameraRotation);
        }
    }
}
