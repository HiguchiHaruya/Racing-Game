using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    private CarState _idle = CarState.Idle;
    [SerializeField]
    private AudioSource _carAudio;
    [SerializeField]
    private AudioClip[] _clip;
    private float _maxVolume = 1.0f;
    private float _minVolume = 0.2f;
    private float _maxPitch = 2.0f;
    private float _minPitch = 0.5f;
    private bool _highSpeed = false;
    void Update()
    {
        SelectSound();
    }

    private void SelectSound()
    {
        //if (Vehicle.Instance.GetCurrentState() != _idle) { return; }
        switch (Vehicle.Instance.GetCurrentState())
        {
            case CarState.Idle:
                PlayEngineSound(0);
                break;
            case CarState.Low:
                PlayEngineSound(1);
                break;
            case CarState.High:
                PlayEngineSound(2);
                break;
        }
    }

    private void PlayEngineSound(int index)
    {
        if (_carAudio == null) return;
        _carAudio.Stop();
        _carAudio.clip = _clip[index];
        _carAudio.Play();

        //float speed = Vehicle.Instance.Torque;
        //float pitch = Mathf.Lerp(_minPitch, _maxPitch, speed / Vehicle.Instance.MaxTorque);
        //_carSound.pitch = pitch;
        //float volume = Mathf.Lerp(_minVolume, _maxVolume, speed / Vehicle.Instance.MaxTorque);
        //_carSound.volume = volume;
    }
}
