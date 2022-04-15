using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour
{
    [HideInInspector]
    public Skill skill;
    public GameObject slot;
    public Text skillName;
    public Text skillCost;

    public GameObject player;

    void Start() {
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;
    }

    public void AddSkill (Skill newSkill)
    {
        skill = newSkill;
        skillName.text = skill.name;
        skillCost.text = skill.cost + " MP";
        slot.SetActive(true);
    }

    public void ClearSlot ()
    {
        skill = null;
        skillName.text = "Skill Name";
        skillCost.text = "0 MP";
        slot.SetActive(false);
    }

    public void UseSkill () {
        if (skill != null) {
            skill.Use();
        }
    }
}
