using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public CurrentPlayerState state;

    // Update is called once per frame
    void Update()
    {
        if (state == CurrentPlayerState.OVERWORLD) {
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
        }
    }
}
