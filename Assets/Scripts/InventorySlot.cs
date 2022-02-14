using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector]
    public Item item;
    public Image icon;
    public GameObject slot;
    public Text itemName;
    public Text description;
    public Text amount;
    public bool isShop;

    public void AddItem (Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        slot.SetActive(true);
        itemName.text = item.name;
        description.text = item.description;
        if (isShop) {
            amount.text = "$" + item.cost;
        } else {
            if (item.isStackable) {
                amount.text = "x" + item.count;
            } else {
                amount.text = "";
            }
        }
    }

    public void ClearSlot ()
    {
        item = null;
        icon.sprite = null;
        slot.SetActive(false);
        itemName.text = "Item Name";
        description.text = "Lorem ipsum dolor sit amet, youreh motherum adipiscing elit, sed do";
        if (isShop) {
            amount.text = "$0";
        } else {
            amount.text = "x0";
        }
    }

    public void UseItem () {
        if (isShop) {
            if (item.cost >= Inventory.arcs) {
                Inventory.instance.AddItem(item);
                Inventory.arcs -= item.cost;
            }
        } else {
            if (item != null) {
                item.Use();
                Inventory.instance.RemoveItem(item);
            }
        }
    }
}
