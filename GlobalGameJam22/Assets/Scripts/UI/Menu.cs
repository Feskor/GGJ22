using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenOptions()
    {

    }

    public void CloseOptions()
    {

    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
