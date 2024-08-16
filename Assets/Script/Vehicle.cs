using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vehicle : MonoBehaviour, ICar
{
    public static Vehicle Instance;
    [SerializeField]
    private float _maxTorque;
    public float angle;
    public float brake;
    protected float _friction = 2f;
    protected float _driftFriction = 0.25f;
    private float _torque = 0;
    private float maxTime = 1f; //最高速度に達するまでの時間
    private float currentTime;
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft;
    private CarState _currentState;
    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
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
        if (frontLeft.motorTorque <= -2400) { _currentState = CarState.High; }
        else if (frontLeft.motorTorque >= -2400 && frontLeft.motorTorque < 0) { _currentState = CarState.Low; }
        else if (frontLeft.motorTorque >= 0) { _currentState = CarState.Idle; }
    }
    public virtual void Precession()
    {
        // Debug.Log($"最高速度に達するまでの時間 : {currentTime}");
        currentTime = Mathf.Min(currentTime, maxTime);
        if (-Input.GetAxis("Vertical") < 0)
        {
            currentTime += Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
        }
        else
        {
            currentTime -= Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
        }
        //_torque = _maxTorque * -Input.GetAxis("Vertical");
        rearLeft.motorTorque = _torque;
        rearRight.motorTorque = _torque;
        frontLeft.motorTorque = _torque;
        frontRight.motorTorque = _torque;
    }
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
        WheelFrictionCurve sidewaysFriction = rearLeft.sidewaysFriction;
        // Debug.Log(sidewaysFriction.stiffness);
        if (Input.GetKey(KeyCode.LeftShift)) { sidewaysFriction.stiffness = _driftFriction; }
        else { sidewaysFriction.stiffness = _friction; }

        rearLeft.sidewaysFriction = sidewaysFriction;
        rearRight.sidewaysFriction = sidewaysFriction;
    }
    ///<summary> 加速機能メソッド</summary>
    public virtual void Acceleration(Rigidbody rb)
    {
        if (!rb.TryGetComponent<Rigidbody>(out var rigidbody)) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            rigidbody.AddForce(-transform.forward * 20000, ForceMode.Impulse);
        }
    }
    public CarState GetCurrentState()
    {
        return _currentState;
    }
}
public enum CarState
{
    Idle,
    Low,
    High
}
