using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CarController : MonoBehaviour, ICar_2
{
    public static CarController Instance;
    public DontUseWheelCollider_CarParameters _carParameters;
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _steeringInput; // 左右の入力を保持
    private float _backInput;
    float steeringSensitivity;
    private bool _isDrifting;     // ドリフト中かどうか
    private float _steeringLimit = 5;
    private float _targetSpeed = 100f;
    private float _forwardInput;
    [SerializeField]
    private WheelCollider _rearRight, _rearLeft, _frontRight, _frontLeft;

    public float currentSpeed;
    public float CurrentSpeed => _currentSpeed;
    public bool IsDrifting => _isDrifting;
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
        steeringSensitivity = _carParameters.steeringSensitivity;
        InputReader.Instance.OnForwardAsObservable()
                   .Subscribe(context =>
                   {
                       _forwardInput = context.ReadValue<float>();
                       HandleMovement(_forwardInput);
                   })
               .AddTo(this);


    }

    private void Update()
    {
        // 入力を受け取る
        ReceiveInput();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            CameraSwitcher.Instance.ActiveCam2();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            CameraSwitcher.Instance.ActiveCam1();
        }
        HandleMovement(_forwardInput);
        HandleSteering();
        BackMovement();
        HandleDrift();
        // Debug.Log(GetCurrentSpeed() * 2);
        //// 速度制御：最大速度を超えないようにする
        if (_rb.velocity.magnitude > _carParameters.maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _carParameters.maxSpeed;
        }
    }

    private void ReceiveInput()
    {
        // 前進の入力値を取得 (Wキー)
        //forwardInput = InputManager.Instance._inputActions.PlayerActionMap.MoveForward.ReadValue<float>();

        // 左右の入力値を取得 (A/Dキー)
        float moveLeft = InputManager.Instance._inputActions.PlayerActionMap.MoveLeft.ReadValue<float>();
        float moveRight = InputManager.Instance._inputActions.PlayerActionMap.MoveRight.ReadValue<float>();
        // 左が押された場合には-1、右が押された場合には1を格納
        _steeringInput = moveRight - moveLeft;
        _backInput = InputManager.Instance._inputActions.PlayerActionMap.Back.ReadValue<float>();

        // ドリフトの入力値を取得 (Shiftキー)
        _isDrifting = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>() > 0;


    }

    public void HandleMovement(float forwardInput)
    {
        float speed = _targetSpeed / 3.6f; // km/hをm/sに変換
        //float targetSpeedForCurrentGear = Mathf.Clamp(speed, 0, _carParameters.gearMaxSpeeds[] / 3.6f); ギアによって速度変える
        // 前進・後退の処理
        float targetSpeed = forwardInput * _carParameters.maxSpeed;
        // 現在の速度と目標速度の間を滑らかに移行（自然な減速を実現）
        if (forwardInput > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // 入力がない場合は減速
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
        }

        // Rigidbodyを使用して物理的に移動
        Vector3 Force = transform.forward * _currentSpeed;
        Force.y = _rb.velocity.y; // 垂直方向の速度を維持する
        //_rb.velocity = Force;
        _rb.AddForce(Force, ForceMode.Acceleration);
    }
    public float GetCurrentSpeed()
    {
        //float wheelRadius = _rearLeft.radius;
        //float avgRpm = (_frontLeft.rpm + _rearRight.rpm + _frontRight.rpm + _rearLeft.rpm) / 4; //各タイヤの1分間の回転数(rpm)の平均
        //_currentSpeed = (2 * Mathf.PI * wheelRadius * avgRpm / 60) * 3.6f; //km/hでの速度
        return _rb.velocity.magnitude * 3.6f;
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
        float turnAmount = _steeringInput * (driftSteering * Time.fixedDeltaTime);
        // Y軸回転の適用
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
        float sensitivity = _isDrifting ? _carParameters.driftSteeringSensitivity : _carParameters.steeringSensitivity;
        float angle = _steeringInput * 65;
        _frontLeft.steerAngle = angle;
        _frontRight.steerAngle = angle;
    }
    float driftSteering;
    float time = 0;
    public void HandleDrift()
    {
        WheelFrictionCurve sidewaysFriction = _rearLeft.sidewaysFriction;
        WheelFrictionCurve forwardFriction = _rearRight.forwardFriction;
        if (_isDrifting)
        {
            time += Time.deltaTime;
            driftSteering = Mathf.Lerp(_carParameters.steeringSensitivity, _carParameters.driftSteeringSensitivity, time / 1.1f);
            sidewaysFriction.stiffness = _carParameters.sideDriftStiffness;
            forwardFriction.stiffness = _carParameters.forwardDriftStiffness;
        }
        else
        {
            driftSteering = _carParameters.steeringSensitivity;
            time = 0;
            forwardFriction.stiffness = _carParameters.forwardNormalStiffness;
            sidewaysFriction.stiffness = _carParameters.sideNormalStiffness;
        }
        _rearLeft.sidewaysFriction = sidewaysFriction;
        _rearRight.sidewaysFriction = sidewaysFriction;
        _rearLeft.forwardFriction = forwardFriction;
        _rearRight.forwardFriction = forwardFriction;
    }
}
