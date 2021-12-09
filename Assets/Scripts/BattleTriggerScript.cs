using UnityEngine;

public class BattleTriggerScript : MonoBehaviour
{
    public GameObject enemy;
    GameObject GameManager;

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Players") {
            GameManager.GetComponent<BattleScript>().BattleTrigger(enemy);
        }
    }
}
