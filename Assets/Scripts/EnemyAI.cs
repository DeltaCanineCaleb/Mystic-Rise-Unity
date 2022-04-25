using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    PlayerState stateEnum;
    [HideInInspector]
    public GameObject player;

    void Start() 
    {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
        player = GameObject.Find("Main Camera").GetComponent<CameraFollow>().player;
    }

    void FixedUpdate()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        string race = this.GetComponent<Character>().characterRace;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD || state == PlayerState.CurrentPlayerState.INVENTORY || state == PlayerState.CurrentPlayerState.DIALOGUE) {
            if (race == "Training Dummy") {
                this.transform.position = new Vector2(23.5f, 7.5f);
            } else if (race == "Slime") {
                float speed = 2f;
                var step = speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, step);
            }
        } else {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }
}
