using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public GameObject slot;
    public Text itemName;
    public Text description;
    public Text amount;

    public void AddItem (Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        slot.SetActive(true);
        itemName.text = item.name;
        description.text = item.description;
        if (item.isStackable) {
            amount.text = "x" + item.count;
        } else {
            amount.text = "";
        }
    }

    public void ClearSlot ()
    {
        item = null;
        icon.sprite = null;
        slot.SetActive(false);
        itemName.text = "Item Name";
        description.text = "Lorem ipsum dolor sit amet, youreh motherum adipiscing elit, sed do";
        amount.text = "x0";
    }

    public void UseItem () {
        if (item != null) {
            item.Use();
            Inventory.instance.RemoveItem(item);
        }
    }
}
