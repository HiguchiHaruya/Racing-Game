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
        if (_rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません！");
        }

        // Rigidbodyの設定を調整して、物理挙動が安定するようにする
        _rb.drag = 0.5f;  // 車の抵抗を少し加えて自然な減速を実現
        _rb.angularDrag = 2f;  // 回転の過剰な累積を防ぐための角度抵抗
        _rb.mass = 2000; // 車両の質量を適切に設定
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
        for (int i = 0; i < _wheelPosition.Length; i++)
        {
            ApplySuspensionForce(_wheelPosition[i]);
        }
        // 速度制御：最大速度を超えないようにする
        if (_rb.velocity.magnitude > _carParameters.maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _carParameters.maxSpeed;
        }
    }

    private void ReceiveInput()
    {
        // 前進・後退の入力値を取得 (Wキー、MoveForward)
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
        //pivot.transform.position = this.transform.position;
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
        float steeringSensitivity = _isDrifting ? _carParameters.driftSteeringSensitivity : _carParameters.steeringSensitivity;

        // ステアリング角度を決定（ドリフト中は感度を高める）
        float turnAmount = _steeringInput * (steeringSensitivity * Time.fixedDeltaTime );

        // Y軸回転の適用
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    public void HandleDrift()
    {
        ////Vector3 driftDirection = transform.right * _steeringInput;
        ////_rb.AddForce(driftDirection * 500 * Time.fixedDeltaTime, ForceMode.Impulse);
        //WheelFrictionCurve sideways = rearLeft.sidewaysFriction;
        //if (_isDrifting)
        //{
        //    sideways.stiffness = 5;
        //}
        //else
        //{
        //    sideways.stiffness = 15;
        //}
        //rearLeft.sidewaysFriction = sideways;
        //rearRight.sidewaysFriction = sideways;
        //Debug.Log($" Stiffness値 :{rearLeft.sidewaysFriction.stiffness}");
    }
    private void ApplySuspensionForce(Transform wheelPos)
    {
        RaycastHit ray;
        if (Physics.Raycast(wheelPos.position, -wheelPos.up, out ray, _suspensionDistance))
        {
            float force = _suspensionStrength * (_suspensionDistance - ray.distance);
            _rb.AddForceAtPosition(wheelPos.up * force, wheelPos.position);
        }
    }
}
