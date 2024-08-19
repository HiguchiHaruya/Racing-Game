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
    private int _lapCount = 0;
    public float angle; //���ړ��p�x
    public float brake; //�u���[�L��
    protected float _friction = 2f; //�ʏ펞�̃^�C���̖��C
    protected float _driftFriction = 0.87f; //�h���t�g���̃^�C���̖��C
    private float _torque = 0; //���݂̑��x
    private float maxTime = 1f; //�ō����x�ɒB����܂ł̎���
    private float currentTime; //maxTime���v�����邽�߂̕ϐ�
    private float _coolTime = 0; //�����̃N�[���^�C��
    protected WheelCollider frontRight, frontLeft, rearRight, rearLeft; //�^�C���B
    private CarState _currentState;
    private bool _isDrifting = false;
    public int LapCount => _lapCount;
    public float MaxTorque => _maxTorque;
    public float Torque => _torque;
    public float CoolTime => _coolTime;
    public bool IsDrifting => _isDrifting;
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
        currentTime = Mathf.Min(currentTime, maxTime);
        if (-Input.GetAxis("Vertical") < 0)
        { //���͒���Max���x�ɒB����܂ł̎��Ԃ��v�Z����_torque�ɒl������
            currentTime += Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
        }
        else
        {
            currentTime -= Time.deltaTime / maxTime;
            _torque = Mathf.Lerp(0, -1 * _maxTorque, currentTime);
            currentTime = Mathf.Max(currentTime, 0); //�v���X�̒l�ɂȂ�Ȃ��悤�ɐ���
        }
        rearLeft.motorTorque = _torque;
        rearRight.motorTorque = _torque;
        frontLeft.motorTorque = _torque;
        frontRight.motorTorque = _torque;
    }
    /// <summary>���ړ����\�b�h </summary>
    public virtual void MoveSideways()
    {
        float steer = angle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;
    }
    public virtual void Breake()
    {
        float brakeforce = Input.GetKey(KeyCode.Space) ? brake : 0;
        frontLeft.brakeTorque = brakeforce;
        frontRight.brakeTorque = brakeforce;
        rearLeft.brakeTorque = brakeforce;
        rearRight.brakeTorque = brakeforce;
    }
    public virtual void Drift()
    {
        _isDrifting = false;
        WheelFrictionCurve sidewaysFriction = rearLeft.sidewaysFriction;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sidewaysFriction.stiffness = _driftFriction;
            _isDrifting = true;
        }
        else { sidewaysFriction.stiffness = _friction; }

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
