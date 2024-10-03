using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarParameters",menuName = "SetCar/CarParameters")]
public class DontUseWheelCollider_CarParameters : ScriptableObject
{
    public float maxSpeed = 200f; //最大速度
    public float acceleration = 10f; //加速度
    public float brakingPower = 50f; //ブレーキの強さ
    public float steeringSensitivety = 2f; //ステアリング角度
    public float driftFactor = 0.8f; //ドリフト
    public float deceleration = 8;
}
