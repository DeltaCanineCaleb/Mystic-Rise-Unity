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
            if (Input.GetButton("Sprint")) {
                speed = 12.5f;
            } else {
                speed = 5f;
            }
            
            // Up and down movement
            if (Input.GetButton("MoveUp")) {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            } else if (Input.GetButton("MoveDown")) {
                rb.velocity = new Vector2(rb.velocity.x, -(speed));
            } else {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            // Left and right movement
            if (Input.GetButton("MoveRight")) {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            } else if (Input.GetButton("MoveLeft")) {
                rb.velocity = new Vector2(-(speed), rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        } else if (state == PlayerState.CurrentPlayerState.BATTLE) {
            // set the player to their battle station wherever that is
            rb.position = battleStation.position;
            rb.velocity = new Vector2(0f, 0f);
        } else if (state == PlayerState.CurrentPlayerState.INVENTORY) {
            rb.velocity = new Vector2(0f, 0f);
        }
    }
}
