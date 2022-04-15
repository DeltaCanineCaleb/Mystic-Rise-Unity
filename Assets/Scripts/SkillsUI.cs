using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    public Transform skillsParent;
    [HideInInspector]
    public GameObject player;
    List<Skill> skills;
    SkillSlot[] slots;

    Skill AddSkillToList(string argument) {
        Skill[] skillsReso = Resources.LoadAll<Skill>("Skills");
        foreach (Skill skill in skillsReso) {
            if (skill.name == argument) {
                Debug.Log("IT MATCHES");
                return skill;
            }
        }
        return null;
    }

    void Start()
    {
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;

        List<string> skillList = player.GetComponent<Character>().skills;
        for (int i = 0; i < skillList.Count; i++) {
            skills.Add(AddSkillToList(skillList[i]));
        }
        
        slots = skillsParent.GetComponentsInChildren<SkillSlot>();

        UpdateUI();
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
