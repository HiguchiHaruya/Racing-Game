using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float maxTorque;
    public float angle;
    public float breake;
    [SerializeField]
    WheelCollider frontRight, frontLeft, rearRight, rearLeft;
    void FixedUpdate()
    {
        Drive();
    }
    void Drive()
    {
        float steer = Input.GetAxis("Horizontal") * angle;
        float torque = -Input.GetAxis("Vertical") * maxTorque;
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;
    }

}
