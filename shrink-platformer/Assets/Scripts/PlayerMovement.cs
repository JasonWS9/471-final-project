using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;



    private Vector2 movement;
    private float playerSpeed;
    [SerializeField] private float defaultPlayerSpeed = 5f;
    [SerializeField] private float sprintingPlayerSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform cameraTransform;

    private bool isSprintPressed;

    bool isJumpPressed = false;

    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpGravity = -10f;
    [SerializeField] float groundedGravity = -3f;

    private bool isGrounded;

    private Vector3 velocity;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        HandleJump();
        HandleSprint();
        Debug.Log("Is Grounded: " + isGrounded);
    }


    private void OnJump(InputValue jumpVal)
    {
        isJumpPressed = jumpVal.Get<float>() > 0;
        Debug.Log(isJumpPressed);
    }



    void HandleJump()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundedGravity;
        }

        if(isJumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * jumpGravity);
            isJumpPressed = false;
        }

        velocity.y = jumpGravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }
    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
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

    private void OnSprint(InputValue sprintVal)
    {
        isSprintPressed = sprintVal.Get<float>() > 0;
    }

    void HandleSprint()
    {
        if (isSprintPressed)
        {
            playerSpeed = sprintingPlayerSpeed;
        } else
        {
            playerSpeed = defaultPlayerSpeed;
        }
    }
}
