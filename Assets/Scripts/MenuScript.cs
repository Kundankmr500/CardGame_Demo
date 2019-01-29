using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{


    // Start the Game on Pressing play Button
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Quit the Game on Pressing Quit Button
    public void QuitGame()
    {
        Application.Quit();
    }


    // Go to Menu Screen on Pressing Menu Button
    public void GotoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
