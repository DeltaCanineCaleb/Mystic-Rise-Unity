using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    [HideInInspector]
    public AudioManager audioManager;

    public int runChance;
    public float waitTime;

    void Awake() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        cameraTransform = playerCamera.transform;
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;
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
            int enemyStation = 2;
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
        audioManager.StopAll();
        audioManager.Play("Battle Start");
        audioManager.Play("Let's Go, Everyone!");
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
        } else {
            return rightTeam;
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
            audioManager.StopAll();
            audioManager.Play("Temo Village");
            StopAllCoroutines();
            return true;
        } else if (rightTeam.Count == 0) {
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            audioManager.StopAll();
            audioManager.Play("Temo Village");
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
        GameObject target = separateTeams(turnOrder, "right")[0];
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == attackButton) {
            panelOfButtons.SetActive(false);
            if (turnOrder[0].GetComponent<Character>().attack > target.GetComponent<Character>().defense) {
                audioManager.Play("Damage");
                battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + (turnOrder[0].GetComponent<Character>().attack - target.GetComponent<Character>().defense) + " damage!";
                target.GetComponent<Character>().currentHP -= (turnOrder[0].GetComponent<Character>().attack - target.GetComponent<Character>().defense);
                yield return new WaitForSeconds(waitTime);
            } else if (turnOrder[0].GetComponent<Character>().attack > 0) {
                audioManager.Play("Null Damage");
                battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for 0 damage!";
                yield return new WaitForSeconds(waitTime);
            }
            // check for if anyone died
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
                audioManager.StopAll();
                audioManager.Play("Temo Village");
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
        if (turnOrder[0].GetComponent<Character>().attack > target.GetComponent<Character>().defense) {
            audioManager.Play("Damage");
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + (turnOrder[0].GetComponent<Character>().attack - target.GetComponent<Character>().defense) + " damage!";
            target.GetComponent<Character>().currentHP -= (turnOrder[0].GetComponent<Character>().attack - target.GetComponent<Character>().defense);
            yield return new WaitForSeconds(waitTime);
        } else if (turnOrder[0].GetComponent<Character>().attack > 0) {
            audioManager.Play("Null Damage");
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for 0 damage!";
            yield return new WaitForSeconds(waitTime);
        }
        // check for if anyone died
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