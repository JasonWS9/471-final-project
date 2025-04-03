using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;



    private Vector2 movement;
    private float playerSpeed;
    [SerializeField] private float defaultPlayerSpeed = 5f;
    [SerializeField] private float sprintingPlayerSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform cameraTransform;

    private bool isSprinting;

    bool isJumpPressed = false;

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();

        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprinting();
    }



    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        Debug.Log(isJumpPressed);
    }

    void handleJump()
    {

    }

    private void Movement()
    {
        float MoveX = movement.x;
        float MoveZ = movement.y;


        Vector3 cameraForward = cameraTransform.forward.normalized;
        Vector3 cameraRight = cameraTransform.right.normalized;
        cameraForward.y = 0; cameraRight.y = 0;

        Vector3 ActualMovement = (cameraForward * MoveZ) + (cameraRight * MoveX);

        if (ActualMovement.magnitude > 0.01f)
        {
            characterController.Move(ActualMovement * Time.deltaTime * playerSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(ActualMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
    }

    private void Sprinting()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = sprintingPlayerSpeed;
        } else
        {
            playerSpeed = defaultPlayerSpeed;
        }
        
    }
}
