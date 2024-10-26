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
        _speedMeter.fillAmount = DontUseWheelCollider_CarController.Instance.GetCurrentSpeed() / 100;
    }

}
