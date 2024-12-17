using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class WheelController : Vehicle, ICar
{
    [SerializeField]
    private float _driftAngle = 10f;
    [SerializeField]
    private float _tiltSpeed = 5f;
    [SerializeField]
    WheelCollider _frontRight, _frontLeft, _rearRight, _rearLeft;
    Rigidbody _rb;
    Transform _carbody;
    private int _firstRun = 0;
    private float _forwardInput;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _carbody = this.transform;
        _rb = GetComponent<Rigidbody>();
        RegisterTire();
        InputReader.Instance.OnMoveForwardAsObservable.Subscribe(context =>
        {
            Debug.Log($"+++ ‘O“ü—Í—ˆ‚Ü‚µ‚½");
            _forwardInput = context.ReadValue<float>();
        }).AddTo(this);
        InputReader.Instance.OnMoveBackAsObservable.Subscribe(context =>
        {
            _forwardInput -= context.ReadValue<float>();
        }).AddTo(this);
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
       // if (!GameManager.Instance.IsGameStart) return;
        Drift();
        MoveSideways();
        Precession(_forwardInput);
        Breake();
        Acceleration(_rb);
        //ApplyCarTilt(_carbody,_driftAngle,_tiltSpeed);
    }
    public override void MoveSideways()
    {
        base.MoveSideways();
    }
    public override void ApplyCarTilt(Transform carBody, float tiltAngle, float tiltSpeed)
    {
        base.ApplyCarTilt(carBody, tiltAngle, tiltSpeed);
    }
    public override void Precession(float input)
    {
        base.Precession(_forwardInput);
    }
    public override void Breake()
    {
        base.Breake();
    }
    public override void Drift()
    {
        base.Drift();
        if (base._isPushDriftButton && _firstRun == 0)
        {
            _rb.AddForce(new Vector3(0, 3500, 0), ForceMode.Impulse);
            _firstRun++;
        }
        else if (!base._isPushDriftButton)
        {
            _firstRun = 0;
        }
    }
    public override void Acceleration(Rigidbody rb)
    {
        base.Acceleration(rb);
    }
}
