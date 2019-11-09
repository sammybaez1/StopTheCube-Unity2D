using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController: MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
