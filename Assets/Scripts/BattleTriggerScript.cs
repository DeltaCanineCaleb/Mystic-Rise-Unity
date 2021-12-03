using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject GameManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Players") {
            GameManager.GetComponent<BattleScript>().BattleTrigger(enemy);
        }
    }
}
