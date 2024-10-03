using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class WheelController : Vehicle, ICar
{
    [SerializeField]
    private float _tiltSpeed = 5f;
    [SerializeField]
    WheelCollider _frontRight, _frontLeft, _rearRight, _rearLeft;
    Rigidbody _rb;
    Transform _carbody;
    private int _firstRun = 0;
    public CarSetting carSetting;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _carbody = this.transform;
        _rb = GetComponent<Rigidbody>();
        RegisterTire();
    }
    private void ApplayCarSettings()
    {
        _rb.centerOfMass = new Vector3(0, carSetting.centerOfMassHeight, 0); //重心を設定する
        //サスペンションの設定を反映
        SetSuspensionSetting(rearLeft);
        SetSuspensionSetting(rearRight);
        SetSuspensionSetting(frontLeft);
        SetSuspensionSetting(frontRight);

    }
    private void SetSuspensionSetting(WheelCollider wheel)
    {
        JointSpring suspensionSpring = wheel.suspensionSpring;
        suspensionSpring.spring = carSetting.suspensionSpring;
        suspensionSpring.damper = carSetting.suspensionDamper;
        wheel.suspensionSpring = suspensionSpring;
    }

    private void RegisterTire()
    {
        base.frontLeft = this._frontLeft;
        base.frontRight = this._frontRight;
        base.rearLeft = this._rearLeft;
        base.rearRight = this._rearRight;

        ApplayCarSettings();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStart) return;
        Drift();
        MoveSideways(carSetting.maxSteerAngle);
        Precession(carSetting.maxTorque);
        Breake();
        Acceleration(_rb);
        //ApplyCarTilt(_carbody,_driftAngle,_tiltSpeed);
    }
    public override void MoveSideways(float angle)
    {
        base.MoveSideways(carSetting.maxSteerAngle);
    }
    public override void ApplyCarTilt(Transform carBody, float tiltAngle, float tiltSpeed)
    {
        base.ApplyCarTilt(carBody, tiltAngle, tiltSpeed);
    }
    public override void Precession(float maxTorque)
    {
        base.Precession(carSetting.maxTorque);
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
