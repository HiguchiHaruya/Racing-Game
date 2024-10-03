using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebind : MonoBehaviour
{
    private Button _rebindButton;
    [SerializeField]
    private string _actionName;
    [SerializeField]
    private GameObject _waitingKeyPanel;
    [SerializeField]
    private Text _currentActionNameText;
    private InputAction _targetAction;
    private void Start()
    {
        _waitingKeyPanel.gameObject.SetActive(false);
        _rebindButton = GetComponent<Button>();
        _targetAction = InputManager.Instance._inputActions.FindAction(_actionName, false); //�w�肵���A�N�V�������擾����
        _currentActionNameText.text = _targetAction.GetBindingDisplayString();
        _rebindButton.onClick.AddListener(StartRebind);
    }
    private void StartRebind()
    {
        InputManager.Instance._inputActions.Disable();
        if (_waitingKeyPanel != null) { _waitingKeyPanel.gameObject.SetActive(true); }
        //���o�C���h����
        _targetAction
            .PerformInteractiveRebinding() //���o�C���h����
            .OnComplete(operation =>
            {
                var currentBinding = operation.action.bindings[0];
                Debug.Log($"�A�N�V���� {currentBinding.name}  {currentBinding.effectivePath}�L�[�ɕύX����܂���");
                Debug.Log($"{_actionName} ���o�C���h��������");
                _currentActionNameText.text = currentBinding.effectivePath;
                if (_waitingKeyPanel != null) { _waitingKeyPanel.gameObject.SetActive(false); }
            })
            .Start(); //���o�C���h�����s
        InputManager.Instance._inputActions.Enable();
    }
}
