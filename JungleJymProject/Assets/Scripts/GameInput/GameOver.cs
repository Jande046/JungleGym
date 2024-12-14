using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with the name of your game scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
