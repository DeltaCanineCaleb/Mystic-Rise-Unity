using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDUpdate : MonoBehaviour
{
    public Character characterObject;
    public Text characterName;
    public Text characterLevel;
    public Slider characterHPBar;

    void Update()
    {
        characterName.text = characterObject.characterName;
        characterLevel.text = "Lv. " + characterObject.characterLevel;
        characterHPBar.maxValue = characterObject.maxHP;
        characterHPBar.value = characterObject.currentHP;
    }
}
