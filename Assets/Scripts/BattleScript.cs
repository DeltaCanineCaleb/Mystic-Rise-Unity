using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentBattleState { START , YOUR_TURN, AWAITING_TURN, CPU_TURN, END}

public class BattleScript : MonoBehaviour
{
    public CurrentBattleState battleState;
    PlayerState.CurrentPlayerState playerState;

    // Start is called before the first frame update
    void BattleStart()
    {
        battleState = CurrentBattleState.START;
        playerState = PlayerState.CurrentPlayerState.BATTLE;
    }
}
