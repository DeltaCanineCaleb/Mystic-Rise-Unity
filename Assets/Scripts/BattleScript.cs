using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentBattleState { START , YOUR_TURN, AWAITING_TURN, CPU_TURN, END}

public class BattleScript : MonoBehaviour
{
    public CurrentBattleState battleState;
    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    void Update()
    {
        playerState = stateEnum.state;
    }
    
    void BattleStart()
    {
        battleState = CurrentBattleState.START;
        playerState = PlayerState.CurrentPlayerState.BATTLE;
    }
}
