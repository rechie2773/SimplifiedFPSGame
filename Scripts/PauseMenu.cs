using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public static bool paused ;
    public GameObject PauseMenuCanvas;

    //audio stop (walking)
    private PlayerMovementAdvanced playerMovement;

    public Slider mouseSensitivitySlider; //adding slider for mouse
    public PlayerCam playerCam;

    void Start()
    {
        Time.timeScale = 1f;
        paused = false;
        playerMovement = FindObjectOfType<PlayerMovementAdvanced>();

        if (mouseSensitivitySlider != null && playerCam != null)
        {
            mouseSensitivitySlider.value = playerCam.senX; // suppose x and y are the same
            mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
        {
            playerMovement.PauseAudio();
        }
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
        {
            playerMovement.ResumeAudio();
        }
    }
    public void UpdateMouseSensitivity(float newSensitivity)
    {
        if (playerCam != null)
        {
            playerCam.SetSensitivityX(newSensitivity);
            playerCam.SetSensitivityY(newSensitivity);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}


