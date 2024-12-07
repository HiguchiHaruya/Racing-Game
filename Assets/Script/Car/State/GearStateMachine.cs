using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStateMachine
{
    private GearState currentGearState;
    public void SetGearState(GearState newState)
    {
        currentGearState?.ExitState(); //現在のステートを終了
        //新ステートに変更
        currentGearState = newState;
        currentGearState.EnterState();
    }
    public void UpdateState()
    {
        currentGearState?.UpdateState();
    }
}
