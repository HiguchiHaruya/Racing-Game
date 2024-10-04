using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontUseWheelCollider_CarController : MonoBehaviour, DontUseWheelCollider_ICar
{
    public static DontUseWheelCollider_CarController Instance;
    public DontUseWheelCollider_CarParameters _carParameters;
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _forwardInput;  // �O�i�E��ނ̓��͂�ێ�
    private float _steeringInput; // ���E�̓��͂�ێ�
    private float _backInput;
    private bool _isDrifting;     // �h���t�g�����ǂ���
    private bool _isForwardInput;
    public float CurrentSpeed => _currentSpeed;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���I");
        }

        // Rigidbody�̐ݒ�𒲐����āA�������������肷��悤�ɂ���
        _rb.drag = 0.5f;  // �Ԃ̒�R�����������Ď��R�Ȍ���������
        _rb.angularDrag = 2f;  // ��]�̉ߏ�ȗݐς�h�����߂̊p�x��R
        _rb.mass = 2000; // �ԗ��̎��ʂ�K�؂ɐݒ�
    }

    private void Update()
    {
        // ���͂��󂯎��
        ReceiveInput();
    }

    private void FixedUpdate()
    {
        // �e����𕨗��t���[���ōs��
        HandleMovement();
        HandleSteering();
        BackMovement();
        if (_isDrifting)
        {
            HandleDrift();
        }

        // ���x����F�ő呬�x�𒴂��Ȃ��悤�ɂ���
        if (_rb.velocity.magnitude > _carParameters.maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _carParameters.maxSpeed;
        }
    }

    private void ReceiveInput()
    {
        // �O�i�E��ނ̓��͒l���擾 (W�L�[�AMoveForward)
        _forwardInput = InputManager.Instance._inputActions.PlayerActionMap.MoveForward.ReadValue<float>();

        // ���E�̓��͒l���擾 (A/D�L�[)
        float moveLeft = InputManager.Instance._inputActions.PlayerActionMap.MoveLeft.ReadValue<float>();
        float moveRight = InputManager.Instance._inputActions.PlayerActionMap.MoveRight.ReadValue<float>();
        // ���������ꂽ�ꍇ�ɂ�-1�A�E�������ꂽ�ꍇ�ɂ�1���i�[
        _steeringInput = moveRight - moveLeft;
        _backInput = InputManager.Instance._inputActions.PlayerActionMap.Back.ReadValue<float>();

        // �h���t�g�̓��͒l���擾 (Shift�L�[)
        _isDrifting = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>() > 0;
    }

    public void HandleMovement()
    {
        //pivot.transform.position = this.transform.position;
        // �O�i�E��ނ̏���
        float targetSpeed = _forwardInput * _carParameters.maxSpeed;

        // ���݂̑��x�ƖڕW���x�̊Ԃ����炩�Ɉڍs�i���R�Ȍ����������j
        if (_forwardInput > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
            _isForwardInput = true;
        }
        else
        {
            // ���͂��Ȃ��ꍇ�͌���
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
            _isForwardInput = false;
        }

        // Rigidbody���g�p���ĕ����I�Ɉړ�
        Vector3 velocity = transform.forward * _currentSpeed;
        velocity.y = _rb.velocity.y; // ���������̑��x���ێ�����
        _rb.velocity = velocity;
    }
    void BackMovement()
    {
        float targetSpeed = _backInput * -_carParameters.maxSpeed;
        if (_backInput > 0 && _forwardInput <= 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
        }
        else if(_backInput <= 0 && _forwardInput <= 0) 
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
        }
    }

    public void HandleSteering()
    {
        // �X�e�A�����O�̏���
        float steeringSensitivity = _isDrifting ? _carParameters.driftSteeringSensitivity : _carParameters.steeringSensitivity;

        // �X�e�A�����O�p�x������i�h���t�g���͊��x�����߂�j
        float turnAmount = _steeringInput * steeringSensitivity * Time.fixedDeltaTime;

        // Y����]�̓K�p
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    public void HandleDrift()
    {
        Vector3 driftDirection = transform.right * _steeringInput; // �E�����ɗ͂�������
        _rb.AddForce(driftDirection * 500 * Time.fixedDeltaTime, ForceMode.Acceleration);

        // �J�E���^�[�X�e�A�����邽�߂ɓ��͂𔽓]���ĉ�����i�h���t�g�����o���j
        if (_steeringInput != 0)
        {
            float counterSteer = _steeringInput * 500 * 0.5f * Time.fixedDeltaTime;
            _rb.AddTorque(Vector3.up * counterSteer, ForceMode.Acceleration);
        }
    }
}
