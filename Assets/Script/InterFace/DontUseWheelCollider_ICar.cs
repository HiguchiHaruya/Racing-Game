using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface DontUseWheelCollider_ICar
{
    /// <summary> �Ԃ̑O��ړ����Ǘ�</summary>
    void HandleMovement();
    /// <summary>�X�e�A�����O���Ǘ�</summary>
    void HandleSteering();
    ///<summary>�h���t�g�������Ǘ�</summary>
    void HandleDrift();
}

