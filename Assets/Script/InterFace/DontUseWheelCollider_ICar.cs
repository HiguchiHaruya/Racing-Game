using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface DontUseWheelCollider_ICar
{
    /// <summary> 車の前後移動を管理</summary>
    void HandleMovement();
    /// <summary>ステアリングを管理</summary>
    void HandleSteering();
    ///<summary>ドリフト処理を管理</summary>
    void HandleDrift();
}

