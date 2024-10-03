using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffectManager : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _leftSlipEffect;
    [SerializeField]
    private TrailRenderer _rightSlipEffect;
    void Update()
    {
        if (Vehicle.Instance.IsDrifting)
        {
            _leftSlipEffect.emitting = true;
            _rightSlipEffect.emitting = true;
        }
        else
        {
            _leftSlipEffect.emitting = false;
            _rightSlipEffect.emitting = false;
        }
    }
}

