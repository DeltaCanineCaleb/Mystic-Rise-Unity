using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // I have No Idea(tm) what this is at all
    public static Inventory instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion
    
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        Item i = items.Find(invItem => item.name == invItem.name);
        if (i == null || !item.isStackable) {
            item.count = 1;
            items.Add(item);
        } else {
            i.count += 1;
        }
        if (onItemChangedCallback != null) {
            onItemChangedCallback.Invoke();
        }
    }

    public void RemoveItem(Item item)
    {
        int count = item.count;
        if (count == 1) {
            items.Remove(item);
        } else {
            item.count -= 1;
        }
        if (onItemChangedCallback != null) {
            onItemChangedCallback.Invoke();
        }
    }
}
