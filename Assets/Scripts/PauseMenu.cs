using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<PlayerMovement>().enabled = true;

        Debug.Log("resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerMovement>().enabled = false;

        Debug.Log("pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Death()
    {
        Pause();
        pauseMenuUI.transform.Find("Resume Button").gameObject.SetActive(false);
        pauseMenuUI.transform.Find("Death Text").gameObject.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("load");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}

