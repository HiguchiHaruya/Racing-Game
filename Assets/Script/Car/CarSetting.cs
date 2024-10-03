using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarSettings", menuName = "Car/Settings", order = 1)]
public class CarSetting : ScriptableObject
{
    public float maxTorque; //�ő�g���N
    public float maxSteerAngle; //�ő�X�e�A�����O�p�x
    public float suspensionSpring; //�T�X�y���V�����̍d��
    public float suspensionDamper; //�T�X�y���V�����̃_���p�[
    public float centerOfMassHeight; //�d�S�̍���
    public float trackWidth; //���E�̃^�C���ƃ^�C���̋���
    public float wheelBase; //�O��̃^�C���ƃ^�C���̋���
}
