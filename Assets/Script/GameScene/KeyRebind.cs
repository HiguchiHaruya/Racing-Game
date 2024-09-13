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
        _targetAction = InputManager.Instance._inputActions.FindAction(actionName, false); //指定したアクションを取得する
        var b = _targetAction.bindings;
        foreach (var binding in b)
        _rebindButton.onClick.AddListener(StartRebind);
    }
    private void StartRebind()
    {
        if (_actionText != null) { _actionText.gameObject.SetActive(true); }
        //リバインドする
        _targetAction
            .PerformInteractiveRebinding() //リバインド準備
            .OnComplete(operation =>
            {
                var currentBinding = operation.action.bindings[0];
               Debug.Log($"アクション {currentBinding.name}  {currentBinding.effectivePath}キーに変更されました");
                Debug.Log($"{actionName} リバインド準備完了");
                if (_actionText != null) { _actionText.gameObject.SetActive(false); }
            })
            .Start(); //リバインドを実行
    }
}
