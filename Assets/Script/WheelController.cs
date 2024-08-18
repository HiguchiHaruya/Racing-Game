using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class WheelController : Vehicle, ICar
{
    [SerializeField]
    WheelCollider _frontRight, _frontLeft, _rearRight, _rearLeft;
    Rigidbody _rb;
   protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        RegisterTire();
    }

    private void RegisterTire()
    {
        base.frontLeft = this._frontLeft;
        base.frontRight = this._frontRight;
        base.rearLeft = this._rearLeft;
        base.rearRight = this._rearRight;
    }
    
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStart) return;
        Drift();
        MoveSideways();
        Precession();
        Breake();
        Acceleration(_rb);
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
    public override void Acceleration(Rigidbody rb)
    {
        base.Acceleration(rb);
    }
}
