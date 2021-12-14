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

    public void AddItem (Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        slot.SetActive(true);
        itemName.text = item.name;
        description.text = item.description;
    }

    public void ClearSlot ()
    {
        item = null;
        icon.sprite = null;
        slot.SetActive(false);
        itemName.text = "Item Name";
        description.text = "Lorem ipsum dolor sit amet, youreh motherum adipiscing elit, sed do";
    }

    public void UseItem () {
        if (item != null) {
            item.Use();
            Inventory.instance.Remove(item);
        }
    }
}
