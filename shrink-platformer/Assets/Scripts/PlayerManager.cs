using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerSize playerSize;

    private int currentCollectibleCount;
    private int totalCollectibleCount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.CompareTag("Collectible"))
        {
            Destroy(hit.gameObject);
            currentCollectibleCount++;

            Debug.Log(currentCollectibleCount);

        }


    }


}
