using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStateMachine
{
    private GearState currentGearState;
    public void SetGearState(GearState newState)
    {
        currentGearState?.ExitState(); //���݂̃X�e�[�g���I��
        //�V�X�e�[�g�ɕύX
        currentGearState = newState;
        currentGearState.EnterState();
    }
    public void UpdateState()
    {
        currentGearState?.UpdateState();
    }
}
