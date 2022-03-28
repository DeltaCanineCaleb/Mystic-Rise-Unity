using UnityEngine;
using UnityEngine.UI;

public class BattleHUDUpdate : MonoBehaviour
{
    public Character characterObject;
    public Text characterName;
    public Text characterLevel;
    public Slider characterHPBar;
    public Text characterHPText;
    public Slider characterMPBar;
    public Text characterMPText;
    public bool isPlayer = false;


    void Update()
    {
        characterName.text = characterObject.characterName;
        characterLevel.text = "Lv. " + characterObject.characterLevel;
        characterHPBar.maxValue = characterObject.maxHP;
        characterHPBar.value = characterObject.currentHP;
        if (characterObject.maxMP == 0) {
            characterMPBar.maxValue = 1;
        } else {
            characterMPBar.maxValue = characterObject.maxMP;
        }
        characterMPBar.value = characterObject.currentMP;
        if (isPlayer) {
            characterHPText.enabled = true;
            characterMPText.enabled = true;
            characterHPText.text = characterObject.currentHP + "/" + characterObject.maxHP;
            characterMPText.text = characterObject.currentMP + "/" + characterObject.maxMP;
        } else {
            characterHPText.enabled = false;
            characterMPText.enabled = false;
        }
    }
}
