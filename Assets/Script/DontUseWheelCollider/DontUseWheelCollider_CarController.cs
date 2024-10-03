using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontUseWheelCollider_CarController : MonoBehaviour, DontUseWheelCollider_ICar
{
    public DontUseWheelCollider_CarParameters _carParameters;
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _forwardInput;  // �O�i�E��ނ̓��͂�ێ�
    private float _steeringInput; // ���E�̓��͂�ێ�
    private bool _isDrifting;     // �h���t�g�����ǂ���

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
        HandleDrift();

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

        // �h���t�g�̓��͒l���擾 (Shift�L�[)
        _isDrifting = InputManager.Instance._inputActions.PlayerActionMap.Drift.ReadValue<float>() > 0;
    }

    public void HandleMovement()
    {
        // �O�i�E��ނ̏���
        float targetSpeed = _forwardInput * _carParameters.maxSpeed;

        // ���݂̑��x�ƖڕW���x�̊Ԃ����炩�Ɉڍs�i���R�Ȍ����������j
        if (_forwardInput > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _carParameters.acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // ���͂��Ȃ��ꍇ�͌���
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _carParameters.deceleration * Time.fixedDeltaTime);
        }

        // Rigidbody���g�p���ĕ����I�Ɉړ�
        Vector3 velocity = transform.forward * _currentSpeed;
        velocity.y = _rb.velocity.y; // ���������̑��x���ێ�����
        _rb.velocity = velocity;
    }

    public void HandleSteering()
    {
        // �X�e�A�����O�̏���
        float steeringSensitivity = _isDrifting ? _carParameters.steeringSensitivety * 3.0f : _carParameters.steeringSensitivety;

        // �X�e�A�����O�p�x������i�h���t�g���͊��x�����߂�j
        float turnAmount = _steeringInput * steeringSensitivity * Time.fixedDeltaTime;

        // Y����]�̓K�p
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    public void HandleDrift()
    {
        if (_isDrifting)
        {
            // �h���t�g���͖��C�����炵�Ċ���₷������
            _rb.drag = 0.1f;  // �h���t�g���͖��C��������

            // �������Ɍy���͂������Ċ���悤�ȋ����ɂ���
            Vector3 driftForce = transform.right * _steeringInput * _carParameters.driftFactor;
            _rb.AddForce(driftForce, ForceMode.Acceleration);
        }
        else
        {
            // �ʏ�̖��C�ɖ߂�
            _rb.drag = 0.5f;  // �ʏ펞�̒�R�l�ɖ߂�
        }
    }
}
