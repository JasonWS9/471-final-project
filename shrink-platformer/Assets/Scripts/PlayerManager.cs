using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerSize playerSize;

    private int currentCollectibleCount;
    private int totalCollectibleCount;

    public Transform respawnPoint;


    private int health;
    private int maxHealth = 3;

    private bool canTakeDamage = true;
    private float damageCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LoseHealth()
    {
        if (!canTakeDamage)
        {
            return;

        } else {
            health--;
            Debug.Log("Health: " + health);
            StartCoroutine(DamageCheck());
        }
    }

    private IEnumerator DamageCheck()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit == null)
        {
            return;
        }

        if (hit.gameObject.CompareTag("Collectible"))
        {
            Destroy(hit.gameObject);
            currentCollectibleCount++;

            Debug.Log("Collectables: " + currentCollectibleCount);

        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            LoseHealth();
        }

        if (hit.gameObject.CompareTag("ShrunkenEnemy"))
        {
            if (!playerSize.isShrunk)
            {
                return;
            }

            if (playerSize.isShrunk)
            {
                LoseHealth();
            }
        }

        if (hit.gameObject.CompareTag("InstantDeathObstacle"))
        {
            Death();
        }

    }

    private void Death()
    {
        transform.position = respawnPoint.transform.position;
        health = maxHealth;
        if(playerSize.isShrunk)
        {
            playerSize.RegrowPlayer();
        }
    }

}
