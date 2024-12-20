using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string playerName { private get; set; }
    private void Goal()
    {
        SceneTransitionManager.Instance.LoadSceneAsync("ResultScene");
    }
    private void LeaveGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneTransitionManager.Instance.LoadSceneAsync("StartScene");
        }
    }
}

