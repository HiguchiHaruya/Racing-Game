using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _saturatedLine;
    [SerializeField]
    private Image _torqueSlider;
    [SerializeField]
    private Text _torqueText;
    [SerializeField]
    private Slider _coolTimeSlier;
    void Update()
    {
        SpeedoMeter();
        MaxSpeedParticle();
        _coolTimeSlier.value = Vehicle.Instance.CoolTime;
    }

    private void SpeedoMeter()
    {
        _torqueSlider.fillAmount = Mathf.Abs(Vehicle.Instance.Torque) / Vehicle.Instance.MaxTorque;
        _torqueText.text = Mathf.Abs((int)Vehicle.Instance.Torque / 10).ToString();
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
