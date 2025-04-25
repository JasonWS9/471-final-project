using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerSize playerSize;

    public int currentCollectibleCount;
    private int totalCollectibleCount = 100;

    public Transform respawnPoint;


    private int health;
    private int maxHealth = 3;

    private bool canTakeDamage = true;
    private float damageCooldown = 1f;

    private bool hasKey = false;
    private int currentLevel;

    private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;


    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image bananaImage;
    [SerializeField] private Sprite bananaSprite;

    [SerializeField] private Image keyImage;
    [SerializeField] private Sprite keySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;

        scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();
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
            SceneManager.LoadScene("Title Screen");
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void LoseHealth()
    {
        if (!canTakeDamage)
        {
            return;
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
            scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();

            if (currentCollectibleCount >= totalCollectibleCount)
            {
                scoreText.color = Color.yellow;
                
            }
            else
            {
                scoreText.color = Color.white;
            }

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

        if (hit.gameObject.CompareTag("Key"))
        {
            hasKey = true;
            Debug.Log("GotKey");
            Destroy(hit.gameObject);

        }

        if (hit.gameObject.CompareTag("Exit"))
        {
            ExitLevel();
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

        audioSource.PlayOneShot(damageSound);
    }
    private void ExitLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (!hasKey)
        {
            Debug.Log("Need Key");
            return; 
        } else if (hasKey)
        {

            switch (currentScene.name)
            {
                case "SampleScene":
                    SceneManager.LoadScene("Level 1");
                    break;
                case "Level 1":
                    SceneManager.LoadScene("Level 2");
                    break;
                case "Level 2":
                    SceneManager.LoadScene("Level 3");
                    break;
                case "Level 3":
                    SceneManager.LoadScene("Level 4");
                    break;
                case "Level 4":
                    SceneManager.LoadScene("Level 5");
                    break;
            }
            Debug.Log("Level Complete");

            hasKey = false;

        }



    }


}
