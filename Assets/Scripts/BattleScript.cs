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
        // choose funny battle start text depending on the enemy
        battleText.text = turnOrder[1].GetComponent<Character>().characterName + " attacks!";
        // start the battle loop using the turn order
        StartCoroutine(PlayerTurn(turnOrder));
    }

    List<GameObject> separateTeams(List<GameObject> turnOrder, string side)
    {
        List<GameObject> leftTeam = new List<GameObject>();
        List<GameObject> rightTeam = new List<GameObject>();
        foreach (var character in turnOrder)
        {
            if (character.GetComponent<Character>().team == "enemy") {
                rightTeam.Add(character);
            } else {
                leftTeam.Add(character);
            }
        }
        if (side == "left") {
            return leftTeam;
        } else if (side == "right") {
            return rightTeam;
        } else {
            return new List<GameObject>();
        }
    }

    bool checkDeath(List<GameObject> turnOrder) 
    {
        int deathCheckInt = 0;
        foreach (var character in new List<GameObject>(turnOrder))
        {
            if (character.GetComponent<Character>().currentHP <= 0) {
                Destroy(turnOrder[deathCheckInt]);
                turnOrder.RemoveAt(deathCheckInt);
            }
            deathCheckInt += 1;
        }
        List<GameObject> leftTeam = separateTeams(turnOrder, "left");
        List<GameObject> rightTeam = separateTeams(turnOrder, "right");
        if (leftTeam.Count == 0) {
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            StopAllCoroutines();
            return true;
        } else if (rightTeam.Count == 0) {
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            StopAllCoroutines();
            return true;
        } else {
            return false;
        }
    }

    IEnumerator PlayerTurn(List<GameObject> turnOrder)
    {
        if (battleText.text.Contains("damage")) {
            battleText.text = "What will you do?";
        }
        panelOfButtons.SetActive(true);
        var waitForButton = new WaitForUIButtons(attackButton, runButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == attackButton) {
            GameObject target = separateTeams(turnOrder, "right")[0];
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + turnOrder[0].GetComponent<Character>().attack + " damage!";
            target.GetComponent<Character>().currentHP -= turnOrder[0].GetComponent<Character>().attack;
            panelOfButtons.SetActive(false);
            // check for if anyone died
            yield return new WaitForSeconds(waitTime);
            if (!checkDeath(turnOrder)) {
                // move turnOrder
                GameObject playerTurn = turnOrder[0];
                turnOrder.RemoveAt(0);
                turnOrder.Add(playerTurn);
                if (turnOrder[0].GetComponent<Character>().team == "enemy") {
                    StartCoroutine(CPUTurn(turnOrder));
                } else {
                    StartCoroutine(PlayerTurn(turnOrder));
                }
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
                if (!checkDeath(turnOrder)) {
                    // move turnOrder
                    GameObject playerTurn = turnOrder[0];
                    turnOrder.RemoveAt(0);
                    turnOrder.Add(playerTurn);
                    if (turnOrder[0].GetComponent<Character>().team == "enemy") {
                        StartCoroutine(CPUTurn(turnOrder));
                    } else {
                        StartCoroutine(PlayerTurn(turnOrder));
                    }
                }
            }
        }
    }

    IEnumerator CPUTurn(List<GameObject> turnOrder)
    {
        GameObject target = separateTeams(turnOrder, "left")[0];
        battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + turnOrder[0].GetComponent<Character>().attack + " damage!";
        target.GetComponent<Character>().currentHP -= turnOrder[0].GetComponent<Character>().attack;
        // check for if anyone died
        yield return new WaitForSeconds(waitTime);
        if (!checkDeath(turnOrder)) {
            // move turnOrder
            GameObject enemyTurn = turnOrder[0];
            turnOrder.RemoveAt(0);
            turnOrder.Add(enemyTurn);
            if (turnOrder[0].GetComponent<Character>().team == "enemy") {
                StartCoroutine(CPUTurn(turnOrder));
            } else {
                StartCoroutine(PlayerTurn(turnOrder));
            }
        }
    }
}