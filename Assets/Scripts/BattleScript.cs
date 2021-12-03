using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

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
    public Button itemsButton;
    public Button skillsButton;
    public Button runButton;
    public Transform cameraTransform;

    public int runChance;
    public float waitTime;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    void Update()
    {
        playerState = stateEnum.state;
        if (Input.GetKey("b") && playerState == PlayerState.CurrentPlayerState.OVERWORLD) {
            stateEnum.state = PlayerState.CurrentPlayerState.BATTLE;
            BattleStart(null);
        }
    }

    int RandomNumber(int min, int max)
    {
        Random random = new Random(); 
        return random.Next(min, max);
    }   
    
    public void BattleTrigger(GameObject enemy)
    {
        stateEnum.state = PlayerState.CurrentPlayerState.BATTLE;
        BattleStart(enemy);
    }
    
    void BattleStart(GameObject enemy)
    {
        battleState = CurrentBattleState.START;
        // instantiate enemy since no reference to them exists yet
        GameObject enemyOpponent;
        if (enemy == null) {
            enemyOpponent = Instantiate(characterPrefab, battleStations[1].transform.position, Quaternion.identity);
        } else {
            enemyOpponent = enemy;
            cameraTransform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -10f);
            enemy.transform.position = battleStations[1].transform.position;
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        // determine player turn order, based on speeds*
        // player goes first for now
        List<GameObject> turnOrder = new List<GameObject>();
        turnOrder.Add(player);
        turnOrder.Add(enemyOpponent);
        List<GameObject> battleCharacters = new List<GameObject>();
        battleCharacters.AddRange(turnOrder);
        // choose funny battle start text depending on the enemy
        battleText.text = battleCharacters[1].GetComponent<Character>().characterName + " attacks!";
        // start the battle loop using the turn order
        StartCoroutine(PlayerTurn(turnOrder, battleCharacters));
    }

    bool checkDeath(List<GameObject> battleCharacters) 
    {
        if (battleCharacters[0].GetComponent<Character>().currentHP <= 0) {
            Destroy(battleCharacters[0]);
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            StopAllCoroutines();
            return true;
        } else if (battleCharacters[1].GetComponent<Character>().currentHP <= 0) {
            Destroy(battleCharacters[1]);
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            StopAllCoroutines();
            return true;
        } else {
            return false;
        }
    }

    IEnumerator PlayerTurn(List<GameObject> turnOrder, List<GameObject> battleCharacters)
    {
        if (battleText.text.Contains("damage")) {
            battleText.text = "What will you do?";
        }
        panelOfButtons.SetActive(true);
        var waitForButton = new WaitForUIButtons(attackButton, runButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == attackButton) {
            GameObject target = battleCharacters[1];
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + turnOrder[0].GetComponent<Character>().attack + " damage!";
            target.GetComponent<Character>().currentHP -= turnOrder[0].GetComponent<Character>().attack;
            panelOfButtons.SetActive(false);
            // check for if anyone died
            yield return new WaitForSeconds(waitTime);
            if (!checkDeath(battleCharacters)) {
                // move turnOrder
                GameObject playerTurn = turnOrder[0];
                turnOrder.RemoveAt(0);
                turnOrder.Add(playerTurn);
                StartCoroutine(CPUTurn(turnOrder, battleCharacters));
            }
        } else if (waitForButton.PressedButton == runButton) {
            int runRoll = RandomNumber(0,100);
            panelOfButtons.SetActive(false);
            if (runRoll <= runChance) {
                battleText.text = turnOrder[0].GetComponent<Character>().characterName + " fled!";
                yield return new WaitForSeconds(waitTime);
                stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
                StopAllCoroutines();
            } else {
                battleText.text = turnOrder[0].GetComponent<Character>().characterName + " tried to run, but couldn't get away!";
                yield return new WaitForSeconds(waitTime);
                // move turnOrder
                GameObject playerTurn = turnOrder[0];
                turnOrder.RemoveAt(0);
                turnOrder.Add(playerTurn);
                StartCoroutine(CPUTurn(turnOrder, battleCharacters));
            }
        }
    }

    IEnumerator CPUTurn(List<GameObject> turnOrder, List<GameObject> battleCharacters)
    {
        GameObject target = battleCharacters[0];
        battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + turnOrder[0].GetComponent<Character>().attack + " damage!";
        target.GetComponent<Character>().currentHP -= turnOrder[0].GetComponent<Character>().attack;
        // check for if anyone died
        yield return new WaitForSeconds(waitTime);
        if (!checkDeath(battleCharacters)) {
            // move turnOrder
            GameObject playerTurn = turnOrder[0];
            turnOrder.RemoveAt(0);
            turnOrder.Add(playerTurn);
            StartCoroutine(PlayerTurn(turnOrder, battleCharacters));
        }
    }
}