using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.8f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 600f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        Dependencies.Instance.RegisterDependency<PlayerMovement>(this);
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }


    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.fixedDeltaTime);
    }


    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.fixedDeltaTime;


        Vector3 gravityMove = new Vector3(0, velocity.y, 0) * Time.fixedDeltaTime;
        controller.Move(gravityMove);
    }

}
