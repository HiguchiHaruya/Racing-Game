using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarSettings", menuName = "Car/Settings", order = 1)]
public class CarSetting : ScriptableObject
{
    public float maxTorque; //最大トルク
    public float maxSteerAngle; //最大ステアリング角度
    public float suspensionSpring; //サスペンションの硬さ
    public float suspensionDamper; //サスペンションのダンパー
    public float centerOfMassHeight; //重心の高さ
    public float trackWidth; //左右のタイヤとタイヤの距離
    public float wheelBase; //前後のタイヤとタイヤの距離
}
