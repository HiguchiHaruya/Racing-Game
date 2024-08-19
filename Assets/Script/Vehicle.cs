using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vehicle : MonoBehaviour, ICar
{
    public static Vehicle Instance;
    [SerializeField]
    private float _maxTorque; //Max速度
    [SerializeField]
    private int _lapCount = 0;
    public float angle; //横移動角度
    public float brake; //ブレーキ力
    protected float _friction = 2f; //通常時のタイヤの摩擦
    protected float _driftFriction = 0.87f; //ドリフト時のタイヤの摩擦
    private float _torque = 0; //現在の速度
    private float maxTime = 1f; //最高速度に達するまでの時間
    private float currentTime; //maxTimeを計測するための変数
    private float _coolTime = 0; //加速のクールタイム
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft; //タイヤ達
    private CarState _currentState;
    private bool _isDrifting = false;
    public int LapCount => _lapCount;
    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
    public float CoolTime => _coolTime;
    public bool IsDrifting => _isDrifting;
    protected virtual void Awake()
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
        _currentState = CarState.Idle;
    }
    private void Update()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (frontLeft.motorTorque <= -2400) { _currentState = CarState.High; }
        else if (frontLeft.motorTorque >= -2400 && frontLeft.motorTorque < 0) { _currentState = CarState.Low; }
        else if (frontLeft.motorTorque >= 0) { _currentState = CarState.Idle; }
    }
    /// <summary>前移動メソッド</summary>
    public virtual void Precession()
    {
        currentTime = Mathf.Min(currentTime, maxTime);
        if (-Input.GetAxis("Vertical") < 0)
        { //入力中にMax速度に達するまでの時間を計算して_torqueに値を入れる
            currentTime += Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
        }
        else
        {
            currentTime -= Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
            currentTime = Mathf.Max(currentTime, 0); //プラスの値にならないように制限
        }
        rearLeft.motorTorque = _torque;
        rearRight.motorTorque = _torque;
        frontLeft.motorTorque = _torque;
        frontRight.motorTorque = _torque;
    }
    /// <summary>横移動メソッド </summary>
    public virtual void MoveSideways()
    {
        float steer = angle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;
    }
    public virtual void Breake()
    {
        float brakeforce = Input.GetKey(KeyCode.Space) ? brake : 0;
        frontLeft.brakeTorque = brakeforce;
        frontRight.brakeTorque = brakeforce;
        rearLeft.brakeTorque = brakeforce;
        rearRight.brakeTorque = brakeforce;
    }
    public virtual void Drift()
    {
        _isDrifting = false;
        WheelFrictionCurve sidewaysFriction = rearLeft.sidewaysFriction;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sidewaysFriction.stiffness = _driftFriction;
            _isDrifting = true;
        }
        else { sidewaysFriction.stiffness = _friction; }

        rearLeft.sidewaysFriction = sidewaysFriction;
        rearRight.sidewaysFriction = sidewaysFriction;
    }
    ///<summary> 加速機能メソッド</summary>
    public virtual void Acceleration(Rigidbody rb)
    {
        _coolTime += Time.deltaTime;
        if ((int)_coolTime >= 30)
        {
            if (!rb.TryGetComponent<Rigidbody>(out var rigidbody)) { return; }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                rigidbody.AddForce(-transform.forward * 20000, ForceMode.Impulse);
                _coolTime = 0;
            }
        }
    }
    public CarState GetCurrentState()
    {
        return _currentState;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _lapCount++;
        }
    }
}
public enum CarState
{
    Idle,
    Low,
    High
}
