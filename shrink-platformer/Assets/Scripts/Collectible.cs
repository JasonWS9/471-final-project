using UnityEngine;

public class Collectible : MonoBehaviour
{

    private Vector3 rotationVector = new Vector3(0,100,0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
