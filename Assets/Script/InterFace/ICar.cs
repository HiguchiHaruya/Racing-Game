using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICar
{
    void Precession(float torque);
    void MoveSideways(float angle);
    void Breake();
    void Drift();
    void ApplyCarTilt(Transform carBody, float tiltAngle, float tiltSpeed);
    void Acceleration(Rigidbody rb);
}
