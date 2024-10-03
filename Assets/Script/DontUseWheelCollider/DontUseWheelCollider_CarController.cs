using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontUseWheelCollider_CarController : MonoBehaviour, DontUseWheelCollider_ICar
{
    public DontUseWheelCollider_CarParameters _carParameters;
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _forwardInput;  // 前進・後退の入力を保持
    private float _steeringInput; // 左右の入力を保持
    private bool _isDrifting;     // ドリフト中かどうか

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
        // 各動作を物理フレームで行う
        HandleMovement();
        HandleSteering();
        HandleDrift();

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

        // ドリフトの入力値を取得 (Shiftキー)
        _isDrifting = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>() > 0;
    }

    public void HandleMovement()
    {
        // 前進・後退の処理
        float targetSpeed = _forwardInput * _carParameters.maxSpeed;

        // 現在の速度と目標速度の間を滑らかに移行（自然な減速を実現）
        if (_forwardInput > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // 入力がない場合は減速
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
        }

        // Rigidbodyを使用して物理的に移動
        Vector3 velocity = transform.forward * _currentSpeed;
        velocity.y = _rb.velocity.y; // 垂直方向の速度を維持する
        _rb.velocity = velocity;
    }

    public void HandleSteering()
    {
        // ステアリングの処理
        float steeringSensitivity = _isDrifting ? _carParameters.steeringSensitivety * 3.0f : _carParameters.steeringSensitivety;

        // ステアリング角度を決定（ドリフト中は感度を高める）
        float turnAmount = _steeringInput * steeringSensitivity * Time.fixedDeltaTime;

        // Y軸回転の適用
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    public void HandleDrift()
    {
        if (_isDrifting)
        {
            // ドリフト中は摩擦を減らして滑りやすくする
            _rb.drag = 0.1f;  // ドリフト中は摩擦を下げる

            // 横方向に軽い力を加えて滑るような挙動にする
            Vector3 driftForce = transform.right * _steeringInput * _carParameters.driftFactor;
            _rb.AddForce(driftForce, ForceMode.Acceleration);
        }
        else
        {
            // 通常の摩擦に戻す
            _rb.drag = 0.5f;  // 通常時の抵抗値に戻す
        }
    }
}
