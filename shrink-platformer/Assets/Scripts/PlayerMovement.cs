using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   
    CharacterController characterController;



    private Vector2 movement;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform cameraTransform;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
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


    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }
}
