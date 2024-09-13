using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebind : MonoBehaviour
{
    private Button _rebindButton;
    [SerializeField]
    private string actionName;
    [SerializeField]
    private Text _actionText;
    private InputAction _targetAction;
    private void Start()
    {
        _actionText.gameObject.SetActive(false);
        _rebindButton = GetComponent<Button>();
        _targetAction = InputManager.Instance._inputActions.FindAction(actionName, false); //�w�肵���A�N�V�������擾����
        var b = _targetAction.bindings;
        foreach (var binding in b)
        _rebindButton.onClick.AddListener(StartRebind);
    }
    private void StartRebind()
    {
        if (_actionText != null) { _actionText.gameObject.SetActive(true); }
        //���o�C���h����
        _targetAction
            .PerformInteractiveRebinding() //���o�C���h����
            .OnComplete(operation =>
            {
                var currentBinding = operation.action.bindings[0];
               Debug.Log($"�A�N�V���� {currentBinding.name}  {currentBinding.effectivePath}�L�[�ɕύX����܂���");
                Debug.Log($"{actionName} ���o�C���h��������");
                if (_actionText != null) { _actionText.gameObject.SetActive(false); }
            })
            .Start(); //���o�C���h�����s
    }
}
