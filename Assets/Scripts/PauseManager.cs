using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseManager : MonoBehaviour
{
    public GameObject screen;
    public FirstPersonController fps;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            PauseGame(paused);
        }
    }

    private void PauseGame(bool p)
    {
        Cursor.visible = p;
        fps.enabled = !p;

        if (p)
        {
            Cursor.lockState = CursorLockMode.None;
            //Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Time.timeScale = 1;
        }

        screen.SetActive(p);
    }

    public void UnPause()
    {
        PauseGame(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
