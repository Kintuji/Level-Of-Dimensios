using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] GameObject pauseMenu;

    private void Update()
    {
        PauseMenuSystem();
    }

    private void PauseMenuSystem()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
