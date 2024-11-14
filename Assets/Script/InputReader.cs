using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using System;

public class InputReader : Singleton<InputReader>, PlayerInputControls.IPlayerActionMapActions
{
    private Subject<InputAction.CallbackContext> _OnForwardSubject = new();
    private Subject<InputAction.CallbackContext> _OnDriftSubject = new();
    private Subject<InputAction.CallbackContext> _OnRightSubject = new();
    private Subject<InputAction.CallbackContext> _OnLeftSubject = new();
    private Subject<InputAction.CallbackContext> _OnBrakeSubject = new();

    public IObservable<InputAction.CallbackContext> OnForwardAsObservable() => _OnForwardSubject;
    public IObservable<InputAction.CallbackContext> OnDriftAsObservable() => _OnDriftSubject;
    public IObservable<InputAction.CallbackContext> OnRightAsObservable() => _OnRightSubject;
    public IObservable<InputAction.CallbackContext> OnLeftAsObservable() => _OnLeftSubject;
    public IObservable<InputAction.CallbackContext> OnBrakeAsObservable() => _OnBrakeSubject;
    private float _forwardInput;
    private float _rightInput;
    private float _leftInput;
    private PlayerInputControls _controls;
    private void Start()
    {
        _controls = InputManager.Instance._inputActions;
        _controls.PlayerActionMap.Enable();
        _controls.PlayerActionMap.SetCallbacks(this);

        OnForwardAsObservable()
            .Subscribe(context =>
        {
            _forwardInput = context.ReadValue<float>();
        });
        OnRightAsObservable()
            .Subscribe(context =>
        {
            _rightInput = context.ReadValue<float>();
        });
        OnLeftAsObservable()
            .Subscribe(context =>
            {
                _leftInput = context.ReadValue<float>();
            });
    }
    public void OnMoveForward(InputAction.CallbackContext context)
    {
        _OnForwardSubject.OnNext(context);
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        _OnLeftSubject.OnNext(context);
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        _OnRightSubject.OnNext(context);
    }

    public void OnDrift(InputAction.CallbackContext context)
    {
        _OnDriftSubject.OnNext(context);
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        _OnBrakeSubject.OnNext(context);
    }
    public void OnBack(InputAction.CallbackContext context)
    {
        
    }
}
