using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSize : MonoBehaviour
{
    public CameraManager cameraManager;
    public PlayerMovement playerMovement;

    private Transform playerTransform;
    private CharacterController characterController;

    private bool shrinkButtonPressed;
    [HideInInspector] public bool isShrunk = false;
    public float shrinkScale = 0.15f;

    private Vector3 originalScale;
    private float originalCharacterControllerHeight;
    private Vector3 originalCharacterControllerCenter;

    private float originalCharacterControllerStepOffset;
    private float originalCharacterControllerSkinWidth;

    private float shrunkenSkinWidth = 0.02f;

    public LayerMask collisionMask;

    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {

        originalScale = transform.localScale;
        originalCharacterControllerHeight = characterController.height;
        originalCharacterControllerCenter = characterController.center;

        originalCharacterControllerStepOffset = characterController.stepOffset;
        originalCharacterControllerSkinWidth = characterController.skinWidth;

    }

    private float sizeChangeCooldown = 1.5f;
    private bool canChangeSize = true;
    public void OnSizeChange()
    {




        if (!isShrunk && !playerMovement.isShrunken && canChangeSize && characterController.isGrounded)
        {
            ShrinkPlayer();
            StartCoroutine(SizeChangeCooldownRoutine());

        }
        else if (isShrunk && playerMovement.isShrunken && canChangeSize && characterController.isGrounded)
        {
            RegrowPlayer();
            StartCoroutine(SizeChangeCooldownRoutine());

        }
    }

    private IEnumerator SizeChangeCooldownRoutine()
    {
        canChangeSize = false;
        yield return new WaitForSeconds(sizeChangeCooldown);
        canChangeSize = true;
        Debug.Log("Can Change Size Again");
    }

    private void ShrinkPlayer()
    {
        characterController.Move(Vector3.up * 0.5f);
        playerMovement.ForceFall();

        transform.localScale = originalScale * shrinkScale;
        
        characterController.height = originalCharacterControllerHeight;

        characterController.center = originalCharacterControllerCenter;

        characterController.stepOffset = originalCharacterControllerStepOffset * shrinkScale;

        characterController.skinWidth = shrunkenSkinWidth;


      

        isShrunk = true;
        playerMovement.isShrunken = true;
        cameraManager.OnPlayerShrink();

    }

    float radiusMod = 0.65f;
    float bottomMod = 0.5f;

    public void RegrowPlayer()
    {



        //Checks if the player can regrow
        float height = originalCharacterControllerHeight;
        float radius = characterController.radius * radiusMod;
        Vector3 bottom = transform.position + Vector3.up * (radius + bottomMod);
        Vector3 top = bottom + Vector3.up * (height - radius * 2f);

        bool canGrow = !Physics.CheckCapsule(bottom, top, radius, collisionMask);

        if (!canGrow)
        {
            Debug.Log("Cant Grow");
            return;
        }

        //Regrows player
        transform.localScale = originalScale;
        characterController.height = originalCharacterControllerHeight;
        characterController.center = originalCharacterControllerCenter;

        characterController.stepOffset = originalCharacterControllerStepOffset;
        characterController.skinWidth = originalCharacterControllerSkinWidth;


        isShrunk = false;
        playerMovement.isShrunken = false;

        cameraManager.OnPlayerRegrow();

    }

    private void OnDrawGizmosSelected()
{
    if (!Application.isPlaying) return;

    float height = originalCharacterControllerHeight;
    float radius = characterController.radius * radiusMod;

    Vector3 bottom = transform.position + Vector3.up * (radius + bottomMod);
    Vector3 top = bottom + Vector3.up * (height - radius * 2f);

    Gizmos.color = Color.green;

    // Draw the capsule (as two spheres and a line in between)
    Gizmos.DrawWireSphere(bottom, radius);
    Gizmos.DrawWireSphere(top, radius);
    Gizmos.DrawLine(bottom + Vector3.forward * radius, top + Vector3.forward * radius);
    Gizmos.DrawLine(bottom - Vector3.forward * radius, top - Vector3.forward * radius);
    Gizmos.DrawLine(bottom + Vector3.right * radius, top + Vector3.right * radius);
    Gizmos.DrawLine(bottom - Vector3.right * radius, top - Vector3.right * radius);
}

}
