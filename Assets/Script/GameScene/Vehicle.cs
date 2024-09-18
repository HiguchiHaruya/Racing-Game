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
    protected float _friction = 3f; //�ʏ펞�̃^�C���̖��C
    protected float _driftFriction = 2.2f; //�h���t�g����Stiffness
    private float _torque = 0; //���݂̑��x
    private float _maxTime = 1f; //�ō����x�ɒB����܂ł̎���
    private float _currentTime; //maxTime���v�����邽�߂̕ϐ�
    private float _coolTime = 0; //�����̃N�[���^�C��
    private float _driftTransitionSpeed = 8f; //�h���t�g��stiffness�lMax�܂ł̑J�ڎ���
    private float _targetFriction = 2;
    private float _currentStiffness = 2f; //���݂�Stiffness
    private float _effectFriction = 1.39f; //�G�t�F�N�g���o��Stiffness�l
    float steer = 0;
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft; //�^�C���B
    private CarState _currentState;
    private bool _isDrifting = false;
    private bool _isFirstRun = false;
    private float _sliderTorque = 0;
    public int LapCount => _lapCount;
    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
    public float CoolTime => _coolTime;
    public bool IsDrifting => _isDrifting;
    public float SliderTorque => _sliderTorque;
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
        // Debug.Log($"inputValue : {input}");
        if (-input < 0)
        { //���͒���Max���x�ɒB����܂ł̎��Ԃ��v�Z����_torque�ɒl������
            _currentTime += Time.deltaTime / _maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, _currentTime);
            _sliderTorque = Mathf.Lerp(0, 150, _currentTime);
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
        }
        else if (rightInput > 0)
        {
            steer = angle * rightInput;
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
        //Debug.Log(rearLeft.sidewaysFriction.stiffness);
        _isDrifting = false;
        WheelFrictionCurve sidewaysFriction = rearLeft.sidewaysFriction;
        float forceAppPointDistance = rearLeft.forceAppPointDistance;
        _currentStiffness = Mathf.Lerp(_currentStiffness, _targetFriction, Time.deltaTime * _driftTransitionSpeed);
        if (driftInput > 0)
        {
            _targetFriction = _driftFriction;
            sidewaysFriction.stiffness = _currentStiffness;
            if (sidewaysFriction.stiffness <= _driftFriction + 0.01) { _isDrifting = true; }
            forceAppPointDistance = 0.125f;
        }
        else
        {
            forceAppPointDistance = 0.075f;
            _currentStiffness = _friction;
            sidewaysFriction.stiffness = _friction;
        }
        rearLeft.forceAppPointDistance = forceAppPointDistance;
        rearRight.forceAppPointDistance = forceAppPointDistance;
        rearLeft.sidewaysFriction = sidewaysFriction;
        rearRight.sidewaysFriction = sidewaysFriction;
    }
    ///<summary> �����@�\���\�b�h</summary>
    public virtual void Acceleration(Rigidbody rb)
    {
        _coolTime += Time.deltaTime;
        if ((int)_coolTime >= 30)
        {
            if (!rb.TryGetComponent<Rigidbody>(out var rigidbody)) { return; }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                rigidbody.AddForce(-transform.forward * 20000, ForceMode.Impulse);
                _coolTime = 0;
            }
        }
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
