using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryNight : MonoBehaviour
{
    public void OnClick() // Function to handle button click
    {
        SceneManager.LoadScene("FPSFog");
    }
    public void MainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
