using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
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
