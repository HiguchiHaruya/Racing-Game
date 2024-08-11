using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarEvent : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _saturatedLine;
    [SerializeField]
    private Image TorqueSlider;
    void Update()
    {
        TorqueSlider.fillAmount =  Mathf.Abs(Vehicle.Instance.Torque) / (Mathf.Abs(Vehicle.Instance.Torque)/ 100);
        Debug.Log($"current : {Vehicle.Instance.Torque} max : {Vehicle.Instance.MaxTorque}");
        if (Mathf.Abs(Vehicle.Instance.Torque) >= Vehicle.Instance.MaxTorque)
        {
            _saturatedLine.Play();
        }
        else { _saturatedLine.Stop(); }
    }
}
