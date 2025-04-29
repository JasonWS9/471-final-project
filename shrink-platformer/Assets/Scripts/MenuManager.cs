using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private bool isPaused = false;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject needKeyPopUp;
    
    public PlayerManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Scene currentScene = SceneManager.GetActiveScene();
        needKeyPopUp.SetActive(false);
        if (currentScene.name == "TitleScreen" || currentScene.name == "TutorialScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LoadGame(int level)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        switch (level)
        {
            case 1:
                SceneManager.LoadScene("Level 1");
                playerManager.totalCollectibleCount = playerManager.level1TotalCollectibleCount;
                break;
            case 2:
                SceneManager.LoadScene("Level 2");
                playerManager.totalCollectibleCount = playerManager.level2TotalCollectibleCount;

                break;
            case 3:
                SceneManager.LoadScene("Level 3");
                playerManager.totalCollectibleCount = playerManager.level3TotalCollectibleCount;

                break;
            case 4:
                SceneManager.LoadScene("Level 4");
                playerManager.totalCollectibleCount = playerManager.level4TotalCollectibleCount;

                break;
            case 5:
                SceneManager.LoadScene("Level 5");
                playerManager.totalCollectibleCount = playerManager.level5TotalCollectibleCount;
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void PauseGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "TitleScreen" && currentScene.name != "TutorialScene")
        {
            if (!isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
            else if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
            }
        }

    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1;
    }

    public IEnumerator NeedKey()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "TitleScreen")
        {
            needKeyPopUp.SetActive(true);
            yield return new WaitForSeconds(2);
            needKeyPopUp.SetActive(false);
        }
           
    }
}
