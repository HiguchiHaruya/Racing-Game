using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour, ICar
{
    public float maxTorque;
    public float angle;
    public float brake;
    protected float _friction = 1f;
    protected float _driftFriction = 0.25f;
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft;
    public virtual void Precession()
    {
        float torque = maxTorque * -Input.GetAxis("Vertical");
        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;
        frontLeft.motorTorque = torque;
        frontRight.motorTorque = torque;
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
