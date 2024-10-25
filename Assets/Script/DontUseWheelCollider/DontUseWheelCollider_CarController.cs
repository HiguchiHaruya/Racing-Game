using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontUseWheelCollider_CarController : MonoBehaviour, DontUseWheelCollider_ICar
{
    public static DontUseWheelCollider_CarController Instance;
    public DontUseWheelCollider_CarParameters _carParameters;
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _forwardInput;  // 前進・後退の入力を保持
    private float _steeringInput; // 左右の入力を保持
    private float _backInput;
    private bool _isDrifting;     // ドリフト中かどうか
    private bool _isForwardInput;
    private float _suspensionStrength = 10000f;
    private float _suspensionDistance = 0.5f;
    [SerializeField]
    private Transform[] _wheelPosition;
    [SerializeField]
    private WheelCollider _rearRight, _rearLeft;
    public float CurrentSpeed => _currentSpeed;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 入力を受け取る
        ReceiveInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        BackMovement();
        HandleDrift();
        //// 速度制御：最大速度を超えないようにする
        if (_rb.velocity.magnitude > _carParameters.maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _carParameters.maxSpeed;
        }
    }

    private void ReceiveInput()
    {
        // 前進・後退の入力値を取得 (Wキー/Sキー)
        _forwardInput = InputManager.Instance._inputActions.PlayerActionMap.MoveForward.ReadValue<float>();

        // 左右の入力値を取得 (A/Dキー)
        float moveLeft = InputManager.Instance._inputActions.PlayerActionMap.MoveLeft.ReadValue<float>();
        float moveRight = InputManager.Instance._inputActions.PlayerActionMap.MoveRight.ReadValue<float>();
        // 左が押された場合には-1、右が押された場合には1を格納
        _steeringInput = moveRight - moveLeft;
        _backInput = InputManager.Instance._inputActions.PlayerActionMap.Back.ReadValue<float>();

        // ドリフトの入力値を取得 (Shiftキー)
        _isDrifting = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>() > 0;
    }

    public void HandleMovement()
    {
        // 前進・後退の処理
        float targetSpeed = _forwardInput * _carParameters.maxSpeed;
        float decelertion = _isDrifting ? 0 : _carParameters.deceleration;
        // 現在の速度と目標速度の間を滑らかに移行（自然な減速を実現）
        if (_forwardInput > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
            _isForwardInput = true;
        }
        else
        {
            // 入力がない場合は減速
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
            _isForwardInput = false;
        }

        // Rigidbodyを使用して物理的に移動
        Vector3 Force = transform.forward * _currentSpeed;
        Force.y = _rb.velocity.y; // 垂直方向の速度を維持する
        //_rb.velocity = Force;
        _rb.AddForce(Force, ForceMode.Acceleration);
    }
    void BackMovement()
    {
        float targetSpeed = _backInput * -_carParameters.maxSpeed;
        if (_backInput > 0 && _forwardInput <= 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
        }
        else if (_backInput <= 0 && _forwardInput <= 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
        }
    }

    public void HandleSteering()
    {
        // ステアリングの処理
        float steeringSensitivity = _isDrifting ? driftSteering : _carParameters.steeringSensitivity;

        // ステアリング角度を決定（ドリフト中は感度を高める）
        float turnAmount = _steeringInput * (steeringSensitivity * Time.fixedDeltaTime);

        // Y軸回転の適用
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }
    float driftSteering;
    float time = 0;
    public void HandleDrift()
    {
        WheelFrictionCurve sidewaysFriction = _rearLeft.sidewaysFriction;
        WheelFrictionCurve forwardFriction = _rearRight.forwardFriction;
        Debug.Log(sidewaysFriction.stiffness);
        if (_isDrifting)
        {
            time += Time.deltaTime;
            driftSteering = Mathf.Lerp(_carParameters.steeringSensitivity, _carParameters.driftSteeringSensitivity, time / 1.1f);
            sidewaysFriction.stiffness = 0.25f;
            forwardFriction.stiffness = 0.5f;
            sidewaysFriction.stiffness = Mathf.Lerp(5, 0.25f, time / 1.25f);
            forwardFriction.stiffness = Mathf.Lerp(5, 0.5f, time / 1.25f);
        }
        else
        {
            driftSteering = _carParameters.steeringSensitivity;
            time = 0;
            forwardFriction.stiffness = 5f;
            sidewaysFriction.stiffness = 5f;
        }
        _rearLeft.sidewaysFriction = sidewaysFriction;
        _rearRight.sidewaysFriction = sidewaysFriction;
        _rearLeft.forwardFriction = forwardFriction;
        _rearRight.forwardFriction = forwardFriction;
    }
}
