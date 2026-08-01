using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Declaration of Variables
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform cameraTransform;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Get movement axes and move the player accordingly
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        HandleMovement();
    }

    /// <summary>
    /// Aligns the player to face the same direction as the camera, and moves the player according to the input axes
    /// </summary>
    private void HandleMovement()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();
        Vector3 movementDirection = (forward * verticalInput + right * horizontalInput).normalized;
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
    }
}