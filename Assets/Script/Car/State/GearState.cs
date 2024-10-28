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
    public abstract void EnterState(); //�M�A���ς�������ɌĂ�
    public abstract void UpdateState(); //���t���[���̍X�V
    public abstract void ExitState(); //���̃M�A�Ɉڍs����Ƃ��ɌĂ�
}
