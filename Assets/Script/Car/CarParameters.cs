using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarParameters", menuName = "SetCar/CarParameters")]
public class DontUseWheelCollider_CarParameters : ScriptableObject
{
    public float maxSpeed = 200f; //�ő呬�x
    public float acceleration = 10f; //�����x
    public float brakingPower = 50f; //�u���[�L�̋���
    public float steeringSensitivity = 2f; //�X�e�A�����O�p�x
    public float driftSteeringSensitivity = 60;
    public float driftFactor = 0.8f; //�h���t�g
    public float deceleration = 8;
    public float driftDeceleration = 15;
    public float[] gearMaxSpeeds = { 0, 15, 25, 30, 35 };
    public float[] gearAccelerations = { 0, 0.2f, 0.25f, 0.3f, 0.35f };
}
