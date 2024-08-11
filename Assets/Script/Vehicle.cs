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
    private float maxTime = 2.5f;//Å‚‘¬“x‚É’B‚·‚é‚Ü‚Å‚ÌŽžŠÔ
    private float currentTime;
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft;

    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
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
    public virtual void Precession()
    {
        if (-Input.GetAxis("Vertical") < 0)
        {
            currentTime += Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
        }
        else
        {
            _torque = 0;
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
        Debug.Log(sidewaysFriction.stiffness);
        if (Input.GetKey(KeyCode.LeftShift)) { sidewaysFriction.stiffness = _driftFriction; }
        else { sidewaysFriction.stiffness = _friction; }

        rearLeft.sidewaysFriction = sidewaysFriction;
        rearRight.sidewaysFriction = sidewaysFriction;
    }
}
