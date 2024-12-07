using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFactor : MonoBehaviour
{
    [SerializeField]
    WheelCollider _frontLeftBase, _frontRightBase, _rearLeftBase, _rearRightBase;
    [SerializeField]
    WheelCollider _frontLeft, _frontRight,_rearLeft,_rearRight;
    [SerializeField]
    DontUseWheelCollider_CarParameters _carParameters;
    private void Start()
    {
        float baseFrontWidth = Vector3.Distance(_frontLeftBase.transform.position, _frontRightBase.transform.position);
        float baseRearWidth = Vector3.Distance(_rearLeftBase.transform.position, _frontRightBase.transform.position);
        float baseWheelBase = Vector3.Distance(_frontLeftBase.transform.position, _rearLeftBase.transform.position);

        float targetFrontWidth = Vector3.Distance(_frontLeft.transform.position, _frontRight.transform.position);
        float targetRearWidth = Vector3.Distance(_rearLeft.transform.position, _frontRight.transform.position);
        float targetWheelBase = Vector3.Distance(_frontLeft.transform.position, _rearLeft.transform.position);

        float frontTrackScaleFactor = targetFrontWidth / baseFrontWidth;
        float rearTrackScaleFactor = targetRearWidth / baseRearWidth;
        float wheelBaseScaleFactor = targetWheelBase / baseWheelBase;

        _carParameters.steeringSensitivity *= frontTrackScaleFactor;
    }
}
