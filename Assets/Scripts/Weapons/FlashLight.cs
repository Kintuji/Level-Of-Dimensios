using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlashLight : MonoBehaviour
{
    private Light _light;
    private bool _isFlashLightOn;
    [SerializeField] private float _maxDistanceToBlind;
    [SerializeField] private float _flashLightBattery;
    [SerializeField] private float _batterySpeed;
    private Player _player;

    public bool IsFlashLightOn { get => _isFlashLightOn; set => _isFlashLightOn = value; }
    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (_isFlashLightOn)
        {
            Blind();
            BatterySystem();
            UIManager.instance.UpdateFlashLightBattery(_flashLightBattery, 100);
        }
    }

    public void TurnOnAndOff()
    {
        _isFlashLightOn = !_isFlashLightOn; 
        _light.gameObject.SetActive(_isFlashLightOn);
    }
    private void BatterySystem()
    {
        Mathf.Clamp(_flashLightBattery, 0, 100);

        if (_flashLightBattery > 0)
        {
            _flashLightBattery -= Time.deltaTime * _batterySpeed;
        }
        else
        {
            _isFlashLightOn = false;
            _light.gameObject.SetActive(false);
        }
    }
    public void Blind()
    {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, _maxDistanceToBlind))
            {
                IBlindable enemies = hit.transform.GetComponent<IBlindable>();
                Platforms platforms = hit.transform.GetComponent<Platforms>();
                WallObstacle wallObstacle = hit.transform.GetComponent<WallObstacle>();

                if (enemies != null)
                {
                    Debug.Log("Enemy blind");
                    StartCoroutine(enemies.Blinded());
                }
                if (platforms != null)
                {
                    platforms.ActiveMeshRender();
                }
                if (wallObstacle != null)
                {
                    wallObstacle.DestroyWall();
                }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _maxDistanceToBlind, Color.red, 1);
            }
    }

    public void RestoreBattery()
    {
        _flashLightBattery = 100;
        UIManager.instance.UpdateFlashLightBattery(_flashLightBattery, 100);
    }
}
