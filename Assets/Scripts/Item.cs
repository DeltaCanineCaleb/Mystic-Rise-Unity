using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "Item Name";
    public Sprite icon = null;
    public string description = "Lorem ipsum dolor sit amet, youreh motherum adipiscing elit, sed do";
    public string type = null;
    public int value = 0;
    public bool isStackable = true;
    public int cost = 0;
    [HideInInspector]
    public int count = 1;

    public virtual void Use() {
        Debug.Log("Using " + name);
        if (type == "healing") {
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Heal");
            Character player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
            player.currentHP += value;
            if (player.currentHP > player.maxHP) {
                player.currentHP = player.maxHP;
            }
            Debug.Log("Healed " + value + " HP, now at " + player.currentHP + "/" + player.maxHP);
        } else if (type.Contains("buff")) {
            // format will be buff, [stat to buff]
            Character player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
            if (type.Contains("crit")) {
                player.critRate *= 2;
            }
        } else if (type == "skill") {
            Character player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
        }
    }
}
