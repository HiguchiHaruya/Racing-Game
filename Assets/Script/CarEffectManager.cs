using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _leftSmokeParticle;
    [SerializeField]
    private ParticleSystem _rightSmokeParticle;
    [SerializeField]
    private TrailRenderer _leftSlipEffect;
    [SerializeField]
    private TrailRenderer _rightSlipEffect;
    void Update()
    {
        if (Vehicle.Instance.IsDrifting)
        {
            _leftSmokeParticle.Play();
            _rightSmokeParticle.Play();
            _leftSlipEffect.emitting = true;
            _rightSlipEffect.emitting = true;
        }
        else
        {
            _leftSmokeParticle.Stop();
            _rightSmokeParticle.Stop();
            _leftSlipEffect.emitting = false;
            _rightSlipEffect.emitting = false;
        }
    }
}
