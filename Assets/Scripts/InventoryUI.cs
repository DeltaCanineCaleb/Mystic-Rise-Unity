using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public Text moneyText;
    public bool accessAtWill;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
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

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
    }
}
