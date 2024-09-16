using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    Button _playGameAgainButton;
    private void Start()
    {
        _playGameAgainButton.onClick.AddListener(PlayGameAgain);
    }
    private void PlayGameAgain()
    {
        Destroy(Vehicle.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Destroy(GameManager.Instance._playerCamera);
        SceneTransitionManager.Instance.LoadSceneAsync("GameScene");
    }
}
