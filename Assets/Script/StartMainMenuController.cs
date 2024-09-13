using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _transitionToGameSceneButton;
    [SerializeField]
    private Button _transitionToConfigButton;

    private void Start()
    {
        _transitionToGameSceneButton
            .onClick
            .AddListener(() => SceneTransitionManager.Instance.LoadSceneAsync("GameScene"));

        _transitionToConfigButton
            .onClick
            .AddListener(() => SceneTransitionManager.Instance.LoadSceneAsync(""));
    }
}
