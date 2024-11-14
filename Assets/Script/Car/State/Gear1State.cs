using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear1State : GearState
{
    public Gear1State(CarController car) : base(car) { } //���̃X�e�[�g��car��e�̃R���X�g���N�^�Ăяo���ēn��
    public override void EnterState()
    {
        Debug.Log("EnterGear1");
        //UI�̍X�V�Ƃ����̕ύX�Ƃ�����
    }
    public override void UpdateState()
    {
        car.currentSpeed = Mathf.Lerp(car.currentSpeed, car._carParameters.gearMaxSpeeds[1], car._carParameters.gearAccelerations[1] * Time.deltaTime);
    }
    public override void ExitState()
    {
        Debug.Log("ExsitGear1");
        //�M�A1���瑼�M�A�ɍs���Ƃ��ɉ������������
    }
}

