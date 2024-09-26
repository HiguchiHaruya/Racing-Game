using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vehicle : MonoBehaviour, ICar
{
    public static Vehicle Instance;
    [SerializeField]
    private float _maxTorque; //Max���x
    [SerializeField]
    private int _lapCount = 1;
    public float angle; //���ړ��p�x
    public float brake; //�u���[�L��
    protected float _normalFriction = 5.5f; //�ʏ펞�̃^�C���̖��C
    protected float _driftFriction = 2.75f; //�h���t�g����Stiffness
    private float _torque = 0; //���݂̑��x
    private float _driftTorque = 100; //�h���t�g���̑O�i��
    private float _maxTime = 1f; //�ō����x�ɒB����܂ł̎���
    private float _currentTime; //maxTime���v�����邽�߂̕ϐ�
    private float _coolTime = 0; //�����̃N�[���^�C��
    private float _driftTransitionSpeed = 8f; //�h���t�g��stiffness�lMax�܂ł̑J�ڎ���
    private float _targetFriction = 2;
    private float _currentStiffness = 4f; //���݂�Stiffness
    private float _effectFriction = 1.39f; //�G�t�F�N�g���o��Stiffness�l
    float steer = 0;
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft; //�^�C���B
    private CarState _currentState;
    protected bool _isDrifting = false;
    protected bool _isPushDriftButton = false;
    private bool _isFirstRun = false;
    private float _sliderTorque = 0;
    private int _coolMaxTime = 30;
    private float _normalForceAppPointDistance = 0.05f;
    private float _driftForceAppPointDistance = 0.08f;
    private float _driftAngle = 8f;

    public int LapCount => _lapCount;
    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
    public float CoolTime => _coolTime;
    public bool IsDrifting => _isDrifting;
    public float SliderTorque => _sliderTorque;
    public int CoolMaxTime => _coolMaxTime;
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _currentState = CarState.Idle;
        _lapCount = 1;
    }
    private void Update()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (frontLeft.motorTorque <= -2400) { _currentState = CarState.High; }
        else if (frontLeft.motorTorque >= -2400 && frontLeft.motorTorque < 0) { _currentState = CarState.Low; }
        else if (frontLeft.motorTorque >= 0) { _currentState = CarState.Idle; }
    }
    /// <summary>�O�ړ����\�b�h</summary>
    public virtual void Precession()
    {
        _currentTime = Mathf.Min(_currentTime, _maxTime);
        var input = InputManager.Instance._inputActions.PlayerActionMap.MoveForward.ReadValue<float>();
        Debug.Log($"�O�i����Ղ��� {input}");
        if (-input < 0)
        { //���͒���Max���x�ɒB����܂ł̎��Ԃ��v�Z����_torque�ɒl������
            if (!_isDrifting)
            {
                _currentTime += Time.deltaTime / _maxTime;
                _torque = Mathf.Lerp(0, -1 * _maxTorque, _currentTime);
                _sliderTorque = Mathf.Lerp(0, 150, _currentTime);
            }
            if (_isDrifting)
            {
                //frontLeft.brakeTorque = brake;
                //frontRight.brakeTorque = brake;
                //rearLeft.brakeTorque = brake;
                //rearRight.brakeTorque = brake;
            }
        }
        else
        {
            _currentTime -= Time.deltaTime / _maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, _currentTime);
            _sliderTorque = Mathf.Lerp(0, 150, _currentTime);
            _currentTime = Mathf.Max(_currentTime, 0); //�v���X�̒l�ɂȂ�Ȃ��悤�ɐ���
        }
        rearLeft.motorTorque = _torque;
        rearRight.motorTorque = _torque;
        frontLeft.motorTorque = _torque;
        frontRight.motorTorque = _torque;
    }
    /// <summary>���ړ����\�b�h </summary>
    public virtual void MoveSideways()
    {
        var leftInput = InputManager.Instance._inputActions.PlayerActionMap.MoveLeft.ReadValue<float>();
        var rightInput = InputManager.Instance._inputActions.PlayerActionMap.MoveRight.ReadValue<float>();
        if (leftInput > 0)
        {
            steer = angle * -leftInput;
            if (_isDrifting)
            {
                steer = _driftAngle * -leftInput;
            }
        }
        else if (rightInput > 0)
        {
            steer = angle * rightInput;
            if (_isDrifting)
            {
                steer = _driftAngle * rightInput;
            }
        }
        else
        {
            steer = 0;
        }
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;
    }
    public virtual void ApplyCarTilt(Transform carBody, float tiltAngle, float tiltSpeed)
    {
        float targetTilt = Input.GetAxis("Horizontal") * tiltAngle;
        Vector3 newAngle = carBody.localEulerAngles;
        newAngle.z = Mathf.LerpAngle(carBody.localEulerAngles.z, targetTilt, Time.deltaTime * tiltSpeed); //�X�����X���[�Y�ɂ���ׂ�Lerp���g��
        carBody.localEulerAngles = newAngle; //�ԑ̂̉�]���X�V
    }
    public virtual void Breake()
    {
        var breakeInput = InputManager.Instance._inputActions.PlayerActionMap.Brake.ReadValue<float>();
        float brakeforce = breakeInput > 0 ? brake : 0;
        frontLeft.brakeTorque = brakeforce;
        frontRight.brakeTorque = brakeforce;
        rearLeft.brakeTorque = brakeforce;
        rearRight.brakeTorque = brakeforce;
    }
    public virtual void Drift()
    {
        var driftInput = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>();
        _isDrifting = false;
        Debug.Log($"�h���t�g����Ղ��� {driftInput}");
        WheelFrictionCurve sidewaysFriction = rearLeft.sidewaysFriction;
        if (driftInput > 0)
        {

            SetForceAppDistance(_driftForceAppPointDistance);
            _currentStiffness = Mathf.Lerp(_currentStiffness, _driftFriction, Time.deltaTime * _driftTransitionSpeed);
            //_targetFriction = _driftFriction;
            sidewaysFriction.stiffness = _currentStiffness;
            if (sidewaysFriction.stiffness <= _driftFriction + 0.01) { _isDrifting = true; }
        }
        else
        {
            SetForceAppDistance(_normalForceAppPointDistance);
            _currentStiffness = _normalFriction;
            sidewaysFriction.stiffness = _normalFriction;
        }
        rearLeft.sidewaysFriction = sidewaysFriction;
        rearRight.sidewaysFriction = sidewaysFriction;
    }
    private void SetForceAppDistance(float value)
    {
        rearLeft.forceAppPointDistance = value;
        rearRight.forceAppPointDistance = value;
        frontLeft.forceAppPointDistance = value;
        frontRight.forceAppPointDistance = value;
    }
    ///<summary> �����@�\���\�b�h</summary>
    public virtual void Acceleration(Rigidbody rb)
    {
        _coolTime += Time.deltaTime;
        if ((int)_coolTime >= _coolMaxTime)
        {
            if (!rb.TryGetComponent<Rigidbody>(out var rigidbody)) { return; }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                rigidbody.AddForce(-transform.forward * 20000, ForceMode.Impulse);
                _coolTime = 0;
            }
        }
    }
    ///  <summary>���݂̎Ԃ̑��x��Ԃ��Ă����</summary>
    /// <returns>���݂̎Ԃ̑��x(km/h)</returns>
    public float GetCurrentSpeed()
    {
        float wheelRadius = frontLeft.radius; //�^�C���̔��a
        float avgRpm = (frontLeft.rpm + frontRight.rpm + rearLeft.rpm + rearRight.rpm) / 4; //�e�^�C����rpm(�ꕪ�Ԃ̉�]��)���擾���ĕ��ς𓾂�B�v����Ɏԗւ��ǂ񂾂���]���Ă邩��������
        float speed = 2 * Mathf.PI * wheelRadius * avgRpm / 60; //�^�C���̉�]������Ԃ̑��x(m/s)���v�Z����
        return speed * 3.6f; //m/s��km/h���[�g�����b���L�����[�g�������ɕϊ�
    }
    public CarState GetCurrentState()
    {
        return _currentState;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _lapCount++;
        }
    }
}
public enum CarState
{
    Idle,
    Low,
    High
}
