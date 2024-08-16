using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


//public enum CarState
//{
//    Idle,
//    Low,
//    High
//}
public class WheelController : Vehicle, ICar
{
    [SerializeField]
    WheelCollider _frontRight, _frontLeft, _rearRight, _rearLeft;
    Rigidbody _rb;
    //private CarState _currentState;
   protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        base.frontLeft = this._frontLeft;
        base.frontRight = this._frontRight;
        base.rearLeft = this._rearLeft;
        base.rearRight = this._rearRight;
    }
    void FixedUpdate()
    {
  //      Debug.Log(_currentState);
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
