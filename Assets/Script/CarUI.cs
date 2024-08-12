using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _saturatedLine;
    [SerializeField]
    private Image TorqueSlider;
    [SerializeField]
    private Text TorqueText;
    void Update()
    {
        TorqueSlider.fillAmount = Mathf.Abs(Vehicle.Instance.Torque) / Vehicle.Instance.MaxTorque;
        Debug.Log($"current : {Vehicle.Instance.Torque} max : {Vehicle.Instance.MaxTorque}");
        MaxSpeedParticle();
        TorqueText.text = Mathf.Abs((int)Vehicle.Instance.Torque).ToString();
    }

    private void MaxSpeedParticle()
    {
        if (Mathf.Abs(Vehicle.Instance.Torque) >= Vehicle.Instance.MaxTorque)
        {
            _saturatedLine.Play();
        }
        else { _saturatedLine.Stop(); }
    }
}
