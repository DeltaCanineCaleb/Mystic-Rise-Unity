using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    public Transform skillsParent;
    [HideInInspector]
    public GameObject player;
    InventorySlot[] slots;

    void Start()
    {
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;
        UpdateUI();

        // slots = skillsParent.GetComponentsInChildren<SkillSlot>();
    }


    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
    }
}
