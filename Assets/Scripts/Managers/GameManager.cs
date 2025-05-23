#region Namespaces/Directives

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion


public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool _hideCursor;
    private Player _player;
    private FlashLight _flashLight;
    private SphereWeapon _sphereWeapon;
    private int _keys;
    public static GameManager instance;
    [SerializeField] private List<Transform> _spawns;
    private Transform _currentPlayerSpawn;
    private int _lives;
    [SerializeField] private GameObject _respawnEffect;

    [SerializeField] private PortalOne _portalOne;
    [SerializeField] private GameObject _gameObjectPortalOne;
    [SerializeField] private GameObject _portalLast;

    private bool _levelOneComplete;
    private bool _playerTeleportedForLevelTwo;
    private bool _playerTeleportedForDreamSpawn = false;

    [SerializeField] private GameObject navMeshLink;
    [SerializeField] private ClaireBrain _claireBrain;

    //[SerializeField] List<BobEnemy> _bobEnemyList;

    [SerializeField] private GameObject _Level1;
    [SerializeField] GameObject _Level2;

    private Action _keyAdded;
    #endregion
    #region Propreties
    public int Keys { get => _keys; set => _keys = value; }
    public bool PlayerTeleportForLevelTwo { get => _playerTeleportedForLevelTwo; set => _playerTeleportedForLevelTwo = value; }
    public bool PlayerTeleportedForDreamSpawn { get => _playerTeleportedForDreamSpawn; set => _playerTeleportedForDreamSpawn = value; }
    public Action KeyAdded { get => _keyAdded; set => _keyAdded = value; }
    #endregion
    #region MonoBehaviour
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

        _player = GameObject.Find("Player").GetComponent<Player>();
        _flashLight = GameObject.Find("FlashLight").GetComponent<FlashLight>();
        _sphereWeapon = GameObject.Find("SphereWeapon").GetComponent<SphereWeapon>();
        _sphereWeapon.gameObject.SetActive(false);
        _portalLast.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _player.OnDeath += CheckPlayerDead;
        _portalOne.LevelOneDone += CheckLevelOneWin;
        Key._onKeyCollected += OncollectedKey;
        _claireBrain.KeyRemoved += ActiveNavMeshLink;
    }
    void Start()
	{
		if (_hideCursor)
        {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
        _currentPlayerSpawn = _spawns[0];
        _respawnEffect.SetActive(false);
        _player.IsPlayerAlive = true;
        _lives = 3;
    }

    private void OnDisable()
    {
        _player.OnDeath -= CheckPlayerDead;
        _portalOne.LevelOneDone -= CheckLevelOneWin;
         Key._onKeyCollected += OncollectedKey;
        _claireBrain.KeyRemoved -= ActiveNavMeshLink;
    }

    private void OncollectedKey()
    {
        Addkeys();
        CheckIfHaveAllKeys();
    }
    #endregion
    public void Addkeys()
    {
        _keys++;
        UIManager.instance.UpdateKeys(_keys);
        if (_keys == 5)
        {
            _gameObjectPortalOne.SetActive(true);
            _levelOneComplete = true;
            _Level2.SetActive(true);
        }
    }

    public void ActiveNavMeshLink()
    {
        navMeshLink.SetActive(true);
    }
    #region CheckFunction
    public void CheckPlayerDead()
	{
            _player.RestoreHP();
            _lives--;
            StartCoroutine(Respawn());
            UIManager.instance.UpdateLives(_lives);
        
        if(_lives <= 0)
        {
            Destroy(_player);
            StartCoroutine(LostSystem());
        }
    }
    public void CheckLevelOneWin()
    {
        if (_levelOneComplete == true && _playerTeleportedForLevelTwo == true)
        {
            _flashLight.RestoreBattery();
            _sphereWeapon.gameObject.SetActive(true);
            _player.HaveSphereWeapon = true;
            _currentPlayerSpawn = _spawns[1];
            _Level1.SetActive(false);

            //turn on bob
        }
    }
    public void CheckGameWin()
    {
        if(_playerTeleportedForDreamSpawn == true && _keys == 12)
        {
            _flashLight.gameObject.SetActive(false);
            _sphereWeapon.gameObject.SetActive(false);
            print("end");
            StartCoroutine(WinSystem());
        }
    }
    public void CheckIfHaveAllKeys()
    {
        if (_keys == 12)
        {
            _portalLast.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Coroutins
    private IEnumerator LostSystem()
    {
        UIManager.instance.ShowLostScreen();

        yield return new WaitForSeconds(2.5f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene(0);
    }
    private IEnumerator WinLoading()
    {
        UIManager.instance.ShowWinScreen();

        yield return new WaitForSeconds(2.5f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);

    }

    private IEnumerator WinSystem()
    {
        UIManager.instance.ShowDreamText();
        yield return new WaitForSeconds(4);
        StartCoroutine(WinLoading());

    }
    private IEnumerator Respawn()
    {
        _respawnEffect.SetActive(true);
        _player.IsPlayerAlive = false;
        _player.transform.position = _currentPlayerSpawn.position;

        yield return new WaitForSeconds(3);
        
        _respawnEffect.SetActive(false);
        _player.IsPlayerAlive = true;
    }
    #endregion
}
