using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public OriginalInputControlsClass _inputActions;
    public static InputManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _inputActions = new OriginalInputControlsClass();
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        if (_inputActions != null)
        {
            _inputActions.Enable();
        }
        else { Debug.Log("‚Ê‚é‚Å‚·1"); }
    }
}
