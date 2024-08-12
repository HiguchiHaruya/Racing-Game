using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource _carSound;
    private float _maxVolume = 1.0f;
    private float _minVolume = 0.2f;
    private float _maxPitch = 2.0f;
    private float _minPitch = 0.5f;
    void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        float speed = Vehicle.Instance.Torque;
        float pitch = Mathf.Lerp(_minPitch, _maxPitch, speed / Vehicle.Instance.MaxTorque);
        _carSound.pitch = pitch;
        float volume = Mathf.Lerp(_minVolume, _maxVolume, speed / Vehicle.Instance.MaxTorque);
        _carSound.volume = volume;
    }
}
