using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Declaration of Variables
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    private float horizontalInput;
    private float verticalInput;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerActions playerActions;
   
    void Start()
    {
        playerActions = GetComponent<PlayerActions>();

    }


    void Update()
    {
        if(playerActions.CurrentMode == GameManager.GameMode.Dodging)
        {
            HandleMovementInput();
        }
    }


    /// <summary>
    /// Move the player exclusively on the X axis to dodge boulders
    /// </summary>
    private void HandleMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
    }
}