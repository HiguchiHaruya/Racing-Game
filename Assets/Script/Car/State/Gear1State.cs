using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear1State : GearState
{
    public Gear1State(CarController car) : base(car) { } //このステートのcarを親のコンストラクタ呼び出して渡す
    public override void EnterState()
    {
        Debug.Log("EnterGear1");
        //UIの更新とか音の変更とかする
    }
    public override void UpdateState()
    {
        car.currentSpeed = Mathf.Lerp(car.currentSpeed, car._carParameters.gearMaxSpeeds[1], car._carParameters.gearAccelerations[1] * Time.deltaTime);
    }
    public override void ExitState()
    {
        Debug.Log("ExsitGear1");
        //ギア1から他ギアに行くときに何か処理あれば
    }
}

