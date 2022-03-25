using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public string characterRace;
    public int characterLevel;

    public int attack;
    public int defense;
    // public int speed;
    public int currentHP;
    public int maxHP;
    public string team;
    public GameObject battleHUD;
    public int critRate;
    public int dropMoney;
    public string dropItems;
    public List<string> skills;

    void Start() {
        battleHUD.SetActive(false);
        if (characterRace == "Dragonwolf") {
            skills.Add("Shadow Punch");
        }
    }

    void OnMouseEnter()
    {
        battleHUD.SetActive(true);
    }

    void OnMouseExit()
    {
        battleHUD.SetActive(false);
    }
}
