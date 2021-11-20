using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform battleStation;
    PlayerState stateEnum;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD) {
            // Up and down movement
            if (Input.GetKey("w")) {
                rb.velocity = new Vector2(rb.velocity.x, 5f);
            } else if (Input.GetKey("s")) {
                rb.velocity = new Vector2(rb.velocity.x, -5f);
            } else {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            // Left and right movement
            if (Input.GetKey("d")) {
                rb.velocity = new Vector2(5f, rb.velocity.y);
            } else if (Input.GetKey("a")) {
                rb.velocity = new Vector2(-5f, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        } else if (state == PlayerState.CurrentPlayerState.BATTLE) {
            // set the player to their battle station wherever that is
            rb.position = battleStation.position;
        }
    }
}
