using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _batteryBar;
    [SerializeField] private Text _keys;
    [SerializeField] private Text _livesNumber;
    [SerializeField] private Image _lostScreen;
    [SerializeField] private Image _winScreen;
    [SerializeField] private Text _dreamText;
    public static UIManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void UpdateHP(float health, float maxHealth)
    {
        _healthBar.fillAmount = health / maxHealth;
    }
    public void UpdateFlashLightBattery(float battery, float maxBattery)
    {
        _batteryBar.fillAmount = battery / maxBattery;
    }
    public void UpdateKeys(int keys)
    {
        _keys.text = keys.ToString();
    }
    public void UpdateLives(int lives)
    {
        _livesNumber.text = lives.ToString();
    }
    public void ShowDreamText()
    {
        _dreamText.gameObject.SetActive(true);
    }
    public void ShowLostScreen()
    {
        _lostScreen.gameObject.SetActive(true);
    }
    public void ShowWinScreen()
    {
        _winScreen.gameObject.SetActive(true);
    }
}
