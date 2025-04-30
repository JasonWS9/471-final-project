using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerSize playerSize;
    public MenuManager menuManager;

    private int currentCollectibleCount = 0;
    [HideInInspector] public int totalCollectibleCount;
    public int level1TotalCollectibleCount = 33;
    public int level2TotalCollectibleCount = 60;
    public int level3TotalCollectibleCount = 70;
    public int level4TotalCollectibleCount = 80;
    public int level5TotalCollectibleCount = 90;

    public Transform respawnPoint;

    private bool hasKey = false;
    private int currentLevel;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip collectibleSound;


    [SerializeField] private AudioClip level1Music;
    [SerializeField] private AudioClip level2Music;
    [SerializeField] private AudioClip level3Music;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI keyText;

    private bool needKeyCanPopUp = true;

    [SerializeField] float musicVolume = 1f;

    private Color scoreTextColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreTextColor = scoreText.color;
        sfxSource = GetComponent<AudioSource>();
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(scoreText.text);
        Debug.Log(level1TotalCollectibleCount.ToString());
        Debug.Log("Current scene: " + currentScene.name);
        switch (currentScene.name)
        {
            case "Level 1":
                totalCollectibleCount = level1TotalCollectibleCount;
                scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();
                musicSource.clip = level1Music;
                musicSource.Play();
                break;
            case "Level 2":
                totalCollectibleCount = level2TotalCollectibleCount;
                scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();
                musicSource.clip = level2Music;
                musicSource.Play();
                break;
            case "Level 3":
                totalCollectibleCount = level3TotalCollectibleCount;
                scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();
                musicSource.clip = level3Music;
                musicSource.Play();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuManager.PauseGame();

        }

        if (hasKey)
        {
            keyText.text = "Collected: 1/1";
        }
        else
        {
            keyText.text = "Collected: 0/1";
        }


        if (menuManager.isPaused)
        {
            musicSource.volume = 0;
        }
        else
        {
            musicSource.volume = musicVolume;
        }
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
            Collectible();
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            Death();
        }

        if (hit.gameObject.CompareTag("ShrunkenEnemy"))
        {
            if (!playerSize.isShrunk)
            {
                return;
            }

            if (playerSize.isShrunk)
            {
                Death();
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

    private IEnumerator CollectiblePitchReset()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Pitch Reset");
        sfxSource.pitch = 1.0f;
    }

    private void Death()
    {
        transform.position = respawnPoint.transform.position;
        if(playerSize.isShrunk)
        {
            playerSize.RegrowPlayer();
        }

        sfxSource.PlayOneShot(damageSound);
    }
    private void ExitLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (!hasKey)
        {
            Debug.Log("Need Key");
            if (needKeyCanPopUp == true)
            {
                sfxSource.PlayOneShot(playerSize.cantGrowErrorSound);
                StartCoroutine(menuManager.NeedKey());
                StartCoroutine(NeedKeyCooldown());
            }
            return; 
        } else if (hasKey)
        {

            switch (currentScene.name)
            {
                case "SampleScene":
                    SceneManager.LoadScene("Level 1");
                    totalCollectibleCount = level1TotalCollectibleCount;
                    break;
                case "Level 1":
                    SceneManager.LoadScene("Level 2");
                    totalCollectibleCount = level2TotalCollectibleCount;

                    break;
                case "Level 2":
                    SceneManager.LoadScene("Level 3");
                    totalCollectibleCount = level3TotalCollectibleCount;

                    break;
                case "Level 3":
                    SceneManager.LoadScene("TitleScreen");
                    totalCollectibleCount = level4TotalCollectibleCount;
                    break;
            }
            Debug.Log("Level Complete");

            hasKey = false;
            currentCollectibleCount = 0;

        }

    }

    private void Collectible()
    {
        currentCollectibleCount++;
        scoreText.text = "Collected: " + currentCollectibleCount.ToString() + "/" + totalCollectibleCount.ToString();


        sfxSource.PlayOneShot(collectibleSound);
        sfxSource.pitch += 0.05f;
        StartCoroutine("CollectiblePitchReset");

        if (currentCollectibleCount >= totalCollectibleCount)
        {
            scoreText.color = Color.yellow;

        }
        else
        {
            scoreText.color = scoreTextColor;
        }
    }
    private IEnumerator NeedKeyCooldown()
    {
        needKeyCanPopUp = false;
        yield return new WaitForSeconds(2);
        needKeyCanPopUp = true;

    }

}
