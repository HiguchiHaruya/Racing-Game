using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera _cam1;
    [SerializeField]
    CinemachineVirtualCamera _cam2;
    private void Start()
    {

    }
    IEnumerator FirstCameraEvent()
    {
        ActiveCam1();
        yield return new WaitForSeconds(0.5f);
        ActiveCam2();
    }
    public void ActiveCam1()
    {
        _cam2.Priority = 0;
        _cam1.Priority = 50;
    }
    public void ActiveCam2()
    {
        _cam1.Priority = 0;
        _cam2.Priority = 50;
    }
}
