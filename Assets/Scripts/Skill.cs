using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Battle/Skill")]
public class Skill : ScriptableObject
{
    new public string name = "Skill Name";
    public int cost = 0;
    public string type = null;

    public virtual void Use() {
        Debug.Log("Using " + name);
        if (type.Contains("selfstatus")) {
            // format will be status, [status to inflict]
            Character player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
            string status = type.Split(","[0])[1].Trim();
            player.status.Add(status);
        } else if (type.Contains("heal")) {
            Character player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
            Character target = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player.GetComponent<Character>();
            float mult = float.Parse(type.Split(","[0])[1].Trim());
            target.currentHP += (int)Mathf.Round(player.attack*mult*cost);
        }
    }
}
