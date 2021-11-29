using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentBattleState { START , YOUR_TURN, AWAITING_TURN, CPU_TURN, END}

public class BattleScript : MonoBehaviour
{
    public CurrentBattleState battleState;

    public GameObject characterPrefab;
    public Transform[] battleStations;
    public GameObject player;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    public Text battleText;
    public GameObject panelOfButtons;
    public Button attackButton;

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
        List<GameObject> turnOrder = new List<GameObject>();
        turnOrder.Add(player);
        turnOrder.Add(enemyOpponent);
        List<GameObject> leftSide = new List<GameObject>();
        leftSide.Add(player);
        List<GameObject> rightSide = new List<GameObject>();
        rightSide.Add(enemyOpponent);
        List<GameObject> battleCharacters = new List<GameObject>();
        battleCharacters.AddRange(leftSide);
        battleCharacters.AddRange(rightSide);
        // choose funny battle start text depending on the enemy
        battleText.text = battleCharacters[1].GetComponent<Character>().characterName + " attacks!";
        // start the battle loop using the turn order
        StartCoroutine(PlayerTurn(turnOrder, battleCharacters));
    }

    IEnumerator PlayerTurn(List<GameObject> turnOrder, List<GameObject> battleCharacters)
    {
        panelOfButtons.SetActive(true);
        var waitForButton = new WaitForUIButtons(attackButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == attackButton)
        {
            GameObject target = battleCharacters[1];
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + turnOrder[0].GetComponent<Character>().attack + " damage!";
            target.GetComponent<Character>().currentHP -= battleCharacters[0].GetComponent<Character>().attack;
            panelOfButtons.SetActive(false);
            // check for if anyone died
            // move turnOrder
        }
    }
}
