using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform battleStation;
    float speed;
    PlayerState stateEnum;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD) {
            if (Input.GetKey("left shift")) {
                speed = 12.5f;
            } else {
                speed = 5f;
            }
            
            // Up and down movement
            if (Input.GetKey("w")) {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            } else if (Input.GetKey("s")) {
                rb.velocity = new Vector2(rb.velocity.x, -(speed));
            } else {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            // Left and right movement
            if (Input.GetKey("d")) {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            } else if (Input.GetKey("a")) {
                rb.velocity = new Vector2(-(speed), rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        } else if (state == PlayerState.CurrentPlayerState.BATTLE) {
            // set the player to their battle station wherever that is
            rb.position = battleStation.position;
            rb.velocity = new Vector2(0f, 0f);
        }
    }
}
