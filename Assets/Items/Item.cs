using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "Item Name";
    public Sprite icon = null;
    public string description = "Lorem ipsum dolor sit amet, youreh motherum adipiscing elit, sed do";
    public string type = null;
    public int value = 0;

    public virtual void Use() {
        Debug.Log("Using " + name);
        if (type == "healing") {
            Character player = GameObject.Find("Player").GetComponent<Character>();
            player.currentHP += value;
            if (player.currentHP > player.maxHP) {
                player.currentHP = player.maxHP;
            }
            Debug.Log("Healed " + value + " HP, now at " + player.currentHP + "/" + player.maxHP);
        }
       
    }
}
