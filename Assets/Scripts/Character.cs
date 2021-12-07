using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int characterLevel;

    public int attack;
    // public int defense;
    // public int speed;
    public int currentHP;
    public int maxHP;
    public string team;
    public GameObject battleHUD;

    void Start()
    {
        battleHUD.SetActive(false);
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
