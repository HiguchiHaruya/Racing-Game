using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICar
{
    void Precession();
    void MoveSideways();
    void Breake();
    void Drift();
    void Acceleration(Rigidbody rb);
}
