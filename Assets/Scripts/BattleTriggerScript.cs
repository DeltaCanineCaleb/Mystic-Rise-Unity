using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerScript : MonoBehaviour
{
    public GameObject enemy;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Players") {
            BattleScript.BattleTrigger(enemy);
        }
    }
}
