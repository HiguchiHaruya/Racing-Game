using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    Image _speedMeter;
    void Update()
    {
        Debug.Log(DontUseWheelCollider_CarController.Instance.CurrentSpeed);
        _speedMeter.transform.Rotate(0, 0, -DontUseWheelCollider_CarController.Instance.CurrentSpeed / 100);
    }

}
