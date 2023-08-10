using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /*
    * Zmiana sceny
    */
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    /*
    * Zakończenie gry
    */
    public void Quit()
    {
        Application.Quit();
    }
}
