using UnityEngine;
using UnityEngine.UI;

public class BattleHUDUpdate : MonoBehaviour
{
    public Character characterObject;
    public Text characterName;
    public Text characterLevel;
    public Slider characterHPBar;
    public Text characterHPText;
    public bool isPlayer = false;


    void Update()
    {
        characterName.text = characterObject.characterName;
        characterLevel.text = "Lv. " + characterObject.characterLevel;
        characterHPBar.maxValue = characterObject.maxHP;
        characterHPBar.value = characterObject.currentHP;
        if (isPlayer) {
            characterHPText.enabled = true;
            characterHPText.text = characterObject.currentHP + "/" + characterObject.maxHP;
        } else {
            characterHPText.enabled = false;
        }
    }
}
