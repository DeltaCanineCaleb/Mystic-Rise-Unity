using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public Text moneyText;
    public bool accessAtWill;
    public bool isShop;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();

        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyText != null) {
            moneyText.text = "Arcs: " + Inventory.arcs;
        }
        if (Input.GetButtonDown("Inventory") && accessAtWill)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf) {
                stateEnum.state = PlayerState.CurrentPlayerState.INVENTORY;
            } else {
                stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
            }
        }
    }

    public void UpdateUI()
    {
        if (!isShop) {
            for (int i = 0; i < slots.Length; i++) {
                if (i < inventory.items.Count) {
                    slots[i].AddItem(inventory.items[i]);
                } else {
                    slots[i].ClearSlot();
                }
            }
        } else {
            List<Item> shopStock = GameObject.Find("GameManager").GetComponent<DialogueHandler>().shopStock;
            for (int i = 0; i < slots.Length; i++) {
                if (i < shopStock.Count) {
                    slots[i].AddItem(shopStock[i]);
                } else {
                    slots[i].ClearSlot();
                }
            }
        }
    }
}
