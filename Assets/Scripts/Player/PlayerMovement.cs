using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.8f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float distToGround;
    public Animator WalkAnim;

    [Header("Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 600f;

    [Header("Hand")]
    public GameObject handParent;
    public GameObject axePref;
    public GameObject pickaxePref;
    public string currentItem = null;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (Dependencies.Instance != null)
            Dependencies.Instance.RegisterDependency<PlayerMovement>(this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (MinigameManager.Instance != null && MinigameManager.Instance.IsMinigameActive())
            return; // zablokuj input tylko kiedy minigra jest aktywna
        HandleLook();
        HandleInput();
    }


    void FixedUpdate()
    {
        if (MinigameManager.Instance != null && MinigameManager.Instance.IsMinigameActive())
            return;

        HandleMovement();
        ApplyGravity();
    }


    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = moveSpeed * sprintMultiplier;
        }

        speed = moveSpeed * sprintMultiplier;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentItem = null;
            handParent.transform.DestroyAllChildren();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentItem = "axe";
            handParent.transform.DestroyAllChildren();
            Instantiate(axePref, handParent.transform.position, Quaternion.identity, handParent.transform);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentItem = "pickaxe";
            handParent.transform.DestroyAllChildren();
            Instantiate(pickaxePref, handParent.transform.position, Quaternion.identity, handParent.transform);
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

    public void TakeStep()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
        {
            string GroundType = hit.collider.tag;
        }
    }
}
