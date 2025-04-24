using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame(string level)
    {
        SceneManager.LoadScene(level);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
        } 
        else if (isPaused)
        {

            isPaused = false;
        }
    }

    void EnableSettings()
    {

    }



}
