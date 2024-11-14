using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : Singleton<CameraSwitcher>
{
    [SerializeField]
    private CinemachineVirtualCamera _cam1;
    [SerializeField]
    private CinemachineVirtualCamera _cam2;
    private void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(StartCameraEvent());
    }
    private IEnumerator StartCameraEvent()
    {
        ActiveCam2();
        yield return new WaitForSeconds(0.5f);
        ActiveCam1();
    }
    public void ActiveCam1()
    {
        _cam1.Priority = 10;
        _cam2.Priority = 0;
    }
    public void ActiveCam2()
    {
        _cam1.Priority = 0;
        _cam2.Priority = 10;
    }
}
