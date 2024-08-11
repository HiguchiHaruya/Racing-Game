using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;



public class WheelController : Vehicle, ICar
{
    Rigidbody _rb;
    [SerializeField]
    WheelCollider _frontRight, _frontLeft, _rearRight, _rearLeft;

    private void Start()
    {
        base.frontLeft = this._frontLeft;
        base.frontRight = this._frontRight;
        base.rearLeft = this._rearLeft;
        base.rearRight = this._rearRight;
    }
    void FixedUpdate()
    {
        Drift();
        MoveSideways();
        Precession();
        Breake();
    }
    public override void MoveSideways()
    {
        base.MoveSideways();
    }
    public override void Precession()
    {
        base.Precession();
    }
    public override void Breake()
    {
        base.Breake();
    }
    public override void Drift()
    {
        base.Drift();
    }
}
