using System;
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
    public Camera playerCamera;
    public Text battleText;
    public GameObject panelOfButtons;
    public GameObject itemsMenu;
    [HideInInspector]
    public GameObject player;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;
    
    public Button attackButton;
    public Button itemsButton;
    public Button skillsButton;
    public Button runButton;
    public Button itemsBackButton;
    public Button invSlot1;
    public Button invSlot2;
    public Button invSlot3;
    public Button invSlot4;
    public Button invSlot5;
    public Button invSlot6;

    Transform cameraTransform;
    [HideInInspector]
    public AudioManager audioManager;
    public int runChance;
    public int critRate;
    public float waitTime;

    string ifVowel(String checkString) {
        char letter = checkString.ToLower()[0];
        if (letter == "a"[0] || letter == "e"[0] || letter == "i"[0] || letter == "o"[0] || letter == "u"[0] || letter == "x"[0]) {
            return "n";
        } else {
            return "";
        }
    }

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
    
    string checkForSpecialStart(List<GameObject> turnOrder) {
        if (turnOrder[1].GetComponent<Character>().characterName == "Training Dummy") {
            return "training dummy";
        }
        return null;
    }

    void BattleStart(List<GameObject> enemies)
    {
        List<GameObject> turnOrder = new List<GameObject>();
        turnOrder.Add(player);
        // instantiate enemy since no reference to them exists yet
        GameObject enemyOpponent;
        if (enemies[0] == null) {
            enemyOpponent = Instantiate(characterPrefab, battleStations[2].transform.position, Quaternion.identity);
            enemyOpponent.transform.GetChild(0).GetComponent<Canvas>().worldCamera = playerCamera;
            turnOrder.Add(enemyOpponent);
            enemyOpponent = Instantiate(characterPrefab, battleStations[3].transform.position, Quaternion.identity);
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
                enemyOpponent.GetComponent<Character>().critRate = 1;
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
        switch (checkForSpecialStart(turnOrder)) {
            case "training dummy":
                battleText.text = "The training dummy is just standing there.";
                break;
            default:
                battleText.text = turnOrder[1].GetComponent<Character>().characterName + " attacks!";
                break;
            }
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
                // this will not work when multiplayer comes around
                Inventory.arcs += character.GetComponent<Character>().dropMoney;
                Destroy(turnOrder[deathCheckInt]);
                turnOrder.RemoveAt(deathCheckInt);
            }
            deathCheckInt += 1;
        }
        List<GameObject> leftTeam = separateTeams(turnOrder, "left");
        List<GameObject> rightTeam = separateTeams(turnOrder, "right");
        if (leftTeam.Count == 0 || rightTeam.Count == 0) {
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            audioManager.StopAll();
            audioManager.Play("Temo Village");
            StopAllCoroutines();
            return true;
        } else {
            return false;
        }
    }

    int DamageCalculation(GameObject attacker, GameObject defender, bool isCrit) {
        int attack = attacker.GetComponent<Character>().attack;
        int defense = defender.GetComponent<Character>().defense;
        if (isCrit) {
            return Convert.ToInt32(attack*1.6) - defense;
        } else {
            return attack - defense;
        }
    }

    IEnumerator Attack(List<GameObject> turnOrder, GameObject target) {
        int runRoll = RandomNumber(0,16);
        if (runRoll < turnOrder[0].GetComponent<Character>().critRate && turnOrder[0].GetComponent<Character>().attack > target.GetComponent<Character>().defense) {
            audioManager.Play("Crit Damage");
            battleText.text = "Critical hit! " + turnOrder[0].GetComponent<Character>().characterName + " attacked for " + (DamageCalculation(turnOrder[0], target, true)) + " damage!";
            target.GetComponent<Character>().currentHP -= (DamageCalculation(turnOrder[0], target, true));
            yield return new WaitForSeconds(waitTime);
        } else if (turnOrder[0].GetComponent<Character>().attack > target.GetComponent<Character>().defense) {
            audioManager.Play("Damage");
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for " + (DamageCalculation(turnOrder[0], target, false)) + " damage!";
            target.GetComponent<Character>().currentHP -= (DamageCalculation(turnOrder[0], target, false));
            yield return new WaitForSeconds(waitTime);
        } else if (turnOrder[0].GetComponent<Character>().attack > 0) {
            audioManager.Play("Null Damage");
            battleText.text = turnOrder[0].GetComponent<Character>().characterName + " attacked for 0 damage!";
            yield return new WaitForSeconds(waitTime);
        }
        yield break;
    }

    IEnumerator PlayerTurn(List<GameObject> turnOrder)
    {
        if (battleText.text.Contains("damage") || battleText.text.Contains("used")) {
            battleText.text = "What will you do?";
        }
        panelOfButtons.SetActive(true);
        var waitForButton = new WaitForUIButtons(attackButton, runButton, itemsButton);
        GameObject target = separateTeams(turnOrder, "right")[0];
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == attackButton) {
            panelOfButtons.SetActive(false);
            yield return StartCoroutine(Attack(turnOrder, target));
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
        } else if (waitForButton.PressedButton == itemsButton) {
            panelOfButtons.SetActive(false);
            itemsMenu.SetActive(true);
            var waitForItems = new WaitForUIButtons(itemsBackButton, invSlot1, invSlot2, invSlot3, invSlot4, invSlot5, invSlot6); 
            yield return waitForItems.Reset();
            if (waitForItems.PressedButton == itemsBackButton) {
                panelOfButtons.SetActive(true);
                itemsMenu.SetActive(false);
                StartCoroutine(PlayerTurn(turnOrder));
            // I really wish I could optimize this code I hate it so much [legacy comment]
            } else {
                itemsMenu.SetActive(false);
                string item = waitForItems.PressedButton.transform.parent.GetComponent<InventorySlot>().item.name;
                waitForItems.PressedButton.transform.parent.GetComponent<InventorySlot>().UseItem();
                battleText.text = turnOrder[0].GetComponent<Character>().characterName + " used a" + ifVowel(item) + " " + item + "!";
                yield return new WaitForSeconds(waitTime);
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
            }
        }
    }

    IEnumerator CPUTurn(List<GameObject> turnOrder)
    {
        GameObject target = separateTeams(turnOrder, "left")[0];
        yield return StartCoroutine(Attack(turnOrder, target));
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