using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _saturatedLine;
    [SerializeField]
    private Image _torqueSlider;
    //[SerializeField]
    //private Text _torqueText;
    //[SerializeField]
    //private Slider _coolTimeSlier;
    [SerializeField]
    private Image _driftLamp;
    [SerializeField]
    private Text _lapCountText;
    [SerializeField]
    private Text CurrentTime;
    [SerializeField]
    private ParticleSystem _maxSpeedParticle;
    [SerializeField]
    private Image _currentSpeedMeter;
    [SerializeField]
    private Text _currentSpeedText;
    [SerializeField]
    private Image _coolTimeImage;
    private int t = 150;
    void Update()
    {
        SpeedoMeter();
        MaxSpeedParticle();
        ChangeDriftLampColor();
        DisplayCarData();
    }

    private void DisplayCarData()
    {
        //_coolTimeSlier.value = Vehicle.Instance.CoolTime;
        _coolTimeImage.fillAmount = Vehicle.Instance.CoolTime / Vehicle.Instance.CoolMaxTime;
        if (Vehicle.Instance.LapCount < 4)
        {
            _lapCountText.text = $"LAP {Vehicle.Instance.LapCount.ToString()} / 3";
        }
        if (GameManager.Instance.IsGameStart)
        {
            CurrentTime.text = GameManager.Instance.CurrentGameTime.ToString("F2");
        }
    }

    private void ChangeDriftLampColor()
    {
        if (Vehicle.Instance.IsDrifting)
        {
            _driftLamp.color = Color.red;
        }
        else
        {
            _driftLamp.color = Color.white;
        }
    }

    private void SpeedoMeter()
    {
        _torqueSlider.fillAmount = Mathf.Abs(Vehicle.Instance.Torque) / Vehicle.Instance.MaxTorque;
        Debug.Log(Vehicle.Instance.GetCurrentSpeed());
        //  _torqueText.text = Mathf.Abs((int)Vehicle.Instance.SliderTorque).ToString();
        _currentSpeedText.text = Mathf.Abs((int)Vehicle.Instance.GetCurrentSpeed()).ToString();
        if (_torqueSlider.fillAmount == 1)
        {
            _torqueSlider.color = Color.yellow;
            //_torqueText.color = Color.yellow;
        }
        else
        {
            _torqueSlider.color = Color.white;
            //_torqueText.color = Color.white;
        }
    }

    private void MaxSpeedParticle()
    {
        if (_saturatedLine == null) return;
        if (Mathf.Abs(Vehicle.Instance.Torque) >= Vehicle.Instance.MaxTorque)
        {
            _saturatedLine.Play();
            _maxSpeedParticle.Play();
        }
        else
        {
            _saturatedLine.Stop();
            _maxSpeedParticle.Stop();
        }
    }
}
