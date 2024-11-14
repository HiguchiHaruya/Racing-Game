using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCarManager : MonoBehaviour
{
    private int _firstRunCounter = 0;
    //private void Update()
    //{
    //    switch (Vehicle.Instance.LapCount) //現在のLap数に応じて処理する
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            if (_firstRunCounter == 0)
    //            {
    //                Debug.Log("レコーディング開始");
    //                GhostRecorder.Instance.StartRecording();
    //                _firstRunCounter = 1;
    //            }
    //            break;
    //        case 2:
    //            if (_firstRunCounter == 1)
    //            {
    //                Debug.Log("ゴースト車を走らせます");
    //                GhostCar.Instance.SetGhostData(GhostRecorder.Instance.GetGhostData());
    //                GhostRecorder.Instance.StopRecording();
    //                GhostRecorder.Instance.StartRecording();
    //                _firstRunCounter = 2;
    //            }
    //            break;
    //        case 3:
    //            if (_firstRunCounter == 2)
    //            {
    //                GhostCar.Instance.SetGhostData(GhostRecorder.Instance.GetGhostData());
    //                GhostRecorder.Instance.StopRecording();
    //                _firstRunCounter++;
    //            }
    //            break;
    //    }

    //}
}
