using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Camera _behindCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _behindCamera.depth = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            _behindCamera.depth = -2;
        }
    }
}
