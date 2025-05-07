using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _hTPWindow;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenHTP()
    {
        _hTPWindow.SetActive(true);
    }
    public void CloseHTP()
    {
        _hTPWindow.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
