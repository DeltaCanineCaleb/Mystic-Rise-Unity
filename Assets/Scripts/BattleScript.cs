using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BattleScript : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform[] battleStations;
    public GameObject player;
    public Camera playerCamera;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    public Text battleText;
    public GameObject panelOfButtons;
    public Button attackButton;
    public Button itemsButton;
    public Button skillsButton;
    public Button runButton;
    Transform cameraTransform;

    public int runChance;
    public float waitTime;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
        cameraTransform = playerCamera.transform;
    }

    void Update()
    {
        playerState = stateEnum.state;
        if (Input.GetKey("b") && playerState == PlayerState.CurrentPlayerState.OVERWORLD) {
            stateEnum.state = PlayerState.CurrentPlayerState.BATTLE;
            List<GameObject> opponents = new List<GameObject>();
            opponents.Add(null);
            BattleStart(opponents);
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
        List<GameObject> opponents = new List<GameObject>();
        opponents.Add(enemy);
        BattleStart(opponents);
    }
    
    void BattleStart(List<GameObject> enemies)
    {
        List<GameObject> turnOrder = new List<GameObject>();
        turnOrder.Add(player);
        // instantiate enemy since no reference to them exists yet
        GameObject enemyOpponent;
        if (enemies[0] == null) {
            enemyOpponent = Instantiate(characterPrefab, battleStations[1].transform.position, Quaternion.identity);
            enemyOpponent.transform.GetChild(0).GetComponent<Canvas>().worldCamera = playerCamera;
            turnOrder.Add(enemyOpponent);
            enemyOpponent = Instantiate(characterPrefab, battleStations[2].transform.position, Quaternion.identity);
            enemyOpponent.transform.GetChild(0).GetComponent<Canvas>().worldCamera = playerCamera;
            turnOrder.Add(enemyOpponent);
        } else {
            int enemyStation = 1;
            foreach (var enemy in enemies)
            {
                enemyOpponent = enemy;
                cameraTransform.position = new Vector3(enemyOpponent.transform.position.x, enemyOpponent.transform.position.y, -10f);
                enemyOpponent.transform.position = battleStations[enemyStation].transform.position;
                enemyOpponent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                turnOrder.Add(enemyOpponent);
                enemyStation += 1;
            }
        }
        // determine player turn order, based on speeds*
        // player goes first for now
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