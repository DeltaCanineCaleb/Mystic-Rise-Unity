using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    public Transform skillsParent;
    [HideInInspector]
    public GameObject player;
    List<string> skills;
    SkillSlot[] slots;

    void Start()
    {
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;
        UpdateUI();

        skills = player.GetComponent<Character>().skills;

        // slots = skillsParent.GetComponentsInChildren<SkillSlot>();
    }


    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) {
            if (i < skills.Count) {
                slots[i].AddSkill(skills[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
    }
}
