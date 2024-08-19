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
    private bool _isGameStart = false;
    private bool _isGoal = false;
    private float _currentGameTime = 0f;
    private float _countDownTime = 7;

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
    private void Start()
    {

    }
    private void Update()
    {
        StartCountDown();
        if (!IsGoalMethod())
        {
            _currentGameTime += Time.deltaTime;
        }
        else
        {
            _isGoal = true;
        }
    }

    private bool IsGoalMethod()
    {
        if (Vehicle.Instance.LapCount >= 3)
        {
            return true;
        }
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
