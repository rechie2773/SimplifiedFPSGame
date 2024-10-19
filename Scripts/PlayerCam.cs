using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float senX;
    public float senY;

    public Transform Orientation;

    float xRotate;
    float yRotate;
    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //unlock mouse cursor in GameOver scene
        if (SceneManager.GetActiveScene().name == "MissionSuccess")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (SceneManager.GetActiveScene().name == "GameOverFog")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") *Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotate += mouseX;

        xRotate -= mouseY;

        xRotate = Mathf.Clamp(xRotate, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
        Orientation.rotation = Quaternion.Euler(0, yRotate, 0);
    }
    public void DoFOV(float fov)
    {
        GetComponent<Camera>().DOFieldOfView(fov, 0.25f);
    }
    //sensitivity set
    public void SetSensitivityX(float newSenX)
    {
        senX = newSenX;
    }

    public void SetSensitivityY(float newSenY)
    {
        senY = newSenY;
    }
}
