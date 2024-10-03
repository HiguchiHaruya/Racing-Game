using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarParameters",menuName = "SetCar/CarParameters")]
public class DontUseWheelCollider_CarParameters : ScriptableObject
{
    public float maxSpeed = 200f; //�ő呬�x
    public float acceleration = 10f; //�����x
    public float brakingPower = 50f; //�u���[�L�̋���
    public float steeringSensitivety = 2f; //�X�e�A�����O�p�x
    public float driftFactor = 0.8f; //�h���t�g
    public float deceleration = 8;
}
