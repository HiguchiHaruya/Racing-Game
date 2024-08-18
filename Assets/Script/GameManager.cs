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
    private float _currentGameTime = 0f;
    private float _countDownTime = 7;

    public float CurrentGameTime => _currentGameTime;
    public float CountdownTime => _countDownTime;
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
                    AudioManager.Instance.PlayClip(0);
                    // Debug.Log("4");
                    break;
                case 3:
                    _startLights2.material = _greenMaterial;
                    AudioManager.Instance.PlayClip(1);
                    //  Debug.Log("3");
                    break;
                case 2:
                    _startLights3.material = _greenMaterial;
                    AudioManager.Instance.PlayClip(2);
                    //   Debug.Log("2");
                    break;
                case 1:
                    _isGameStart = true;
                    AudioManager.Instance.PlayClip(3);
                    //  Debug.Log("1");
                    break;
            }
        }
    }
}
