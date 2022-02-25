using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LowPolyDungeonsLite_Overview");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
