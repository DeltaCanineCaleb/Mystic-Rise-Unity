using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentBattleState { START , YOUR_TURN, AWAITING_TURN, CPU_TURN, END}

public class BattleScript : MonoBehaviour
{
    public CurrentBattleState battleState;
    public GameObject characterPrefab;
    public Transform[] battleStations;
    public GameObject player;
    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    void Update()
    {
        playerState = stateEnum.state;
        if (Input.GetKey("b") && playerState == PlayerState.CurrentPlayerState.OVERWORLD) {
            stateEnum.state = PlayerState.CurrentPlayerState.BATTLE;
            BattleStart();
        }
    }
    
    void BattleStart()
    {
        battleState = CurrentBattleState.START;
        // instantiate enemy since no reference to them exists yet
        GameObject enemyOpponent = Instantiate(characterPrefab, battleStations[1].transform.position, Quaternion.identity);
        // determine player turn order, based on speeds*
        // player goes first for now
        GameObject[] turnOrder = new GameObject[] {player, enemyOpponent};
        // start the battle loop using the turn order
    }

    void BattleLoop()
    {
        // if its your turn, YOUR_TURN state (recieve input from this system)
        // if its a friend's turn, AWAITING_TURN state (recieve input from other systems) (wait for multiplayer implementation)
        // if its a CPU's turn, CPU_turn, resolve what happens (multiplayer note: make sure its the same on all sides) and move forward
    }
}
