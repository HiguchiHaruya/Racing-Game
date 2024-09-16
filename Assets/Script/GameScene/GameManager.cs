using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Renderer _startLights1;
    [SerializeField]
    private Renderer _startLights2;
    [SerializeField]
    private Renderer _startLights3;
    [SerializeField]
    private Material _greenMaterial;
    [SerializeField]
    public Camera _playerCamera;
    private bool _isGameStart = true;
    private bool _isGoal = false;
    private float _currentGameTime = 0f;
    private float _countDownTime = 7;
    private int _firstRun = 0;
    public string playerName { private get; set; }
    public float CurrentGameTime => _currentGameTime;
    public float CountdownTime => _countDownTime;
    public bool IsGameStart => _isGameStart;
    public bool IsGoal => _isGoal;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        StartCountDown();
        LeaveGame();
        if (!GetIsGoalFlag())
        {
            _currentGameTime += Time.deltaTime;
        }
        else
        {
            _isGoal = true;
            Debug.Log("ƒS[ƒ‹‚µ‚Ü‚µ‚½");
            Goal();
        }
    }

    private void Goal()
    {
        if (_firstRun == 0)
        {
            _playerCamera.transform.parent = null;
            SceneTransitionManager.Instance.LoadSceneAsync("ResultScene");
            _firstRun++;
        }
    }
    private void LeaveGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneTransitionManager.Instance.LoadSceneAsync("StartScene");
        }
    }
    private bool GetIsGoalFlag()
    {
        if (Vehicle.Instance.LapCount == 4)
        {
            return true;
        }
        if (Input.GetKeyDown(KeyCode.P)) { return true; }
        return false;
    }

    private void StartCountDown()
    {
        if ((int)_countDownTime >= 0)
        {
            _countDownTime -= Time.deltaTime;
            switch ((int)_countDownTime)
            {
                case 4:
                    _startLights1.material = _greenMaterial;
                    break;
                case 3:
                    _startLights2.material = _greenMaterial;
                    break;
                case 2:
                    _startLights3.material = _greenMaterial;
                    break;
                case 1:
                    _isGameStart = true;
                    break;
            }
        }
    }
}
