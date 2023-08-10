using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeIntro : MonoBehaviour
{
    /*
    * Zmiana sceny
    */
    private void OnEnable() {
        SceneManager.LoadScene("Continue");
    }
}
