using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GearState
{
    protected CarController car;
    public GearState(CarController car)
    {
        this.car = car; 
    }
    public abstract void EnterState(); //ギアが変わった時に呼ぶ
    public abstract void UpdateState(); //毎フレームの更新
    public abstract void ExitState(); //他のギアに移行するときに呼ぶ
}
