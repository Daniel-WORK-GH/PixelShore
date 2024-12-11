using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    public float moveSpeed = 8f;
    private bool isControlled = false;
    public float rotationSpeed = 100f; // Speed at which the car rotates

    public Camera mainCamera;

    void Start()
    {

    }

    void Update()
    {
        if (isControlled)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        bool driving = false;

        // Handle movement
        float movement = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            movement = moveSpeed * Time.deltaTime;
            driving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement = -moveSpeed * Time.deltaTime;
            driving = true;
        }

        transform.position += movement * transform.up;

        // Apply rotation
        if (driving)
        {
            float rotation = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                rotation = rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }

            rotation = movement < 0 ? -rotation : rotation;

            // Apply rotation
            transform.Rotate(0, 0, rotation);
        }

        mainCamera.transform.localRotation = Quaternion.Inverse(transform.rotation);
    }

    public void TakeControl()
    {
        isControlled = true;
    }

    public void ReleaseControl()
    {
        isControlled = false;
    }
}