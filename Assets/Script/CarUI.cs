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
    [SerializeField]
    private Image _driftLamp;
    [SerializeField]
    private Text _lapCountText;
    [SerializeField]
    private Text _resultTime;
    void Update()
    {
        SpeedoMeter();
        MaxSpeedParticle();
        ChangeDriftLampColor();
        _coolTimeSlier.value = Vehicle.Instance.CoolTime;
        _lapCountText.text = $"Œ»Ý{Vehicle.Instance.LapCount.ToString()}Žü–Ú!!!!";

        if (GameManager.Instance.IsGoal)
        {
            _resultTime.text = GameManager.Instance.CurrentGameTime.ToString();
        }
    }

    private void ChangeDriftLampColor()
    {
        if (Vehicle.Instance.IsDrifting)
        {
            _driftLamp.color = Color.green;
        }
        else
        {
            _driftLamp.color = Color.white;
        }
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
