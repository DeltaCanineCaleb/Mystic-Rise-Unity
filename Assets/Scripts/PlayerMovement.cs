using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public PhotonView view;

    public Rigidbody2D rb;
    float speed;
    Transform battleStation;
    PlayerState stateEnum;

    bool talk;
    Collider2D window;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
        battleStation = GameObject.Find("Main Camera").transform.GetChild(0).GetChild(0).transform;
    }
    
    void OnTriggerEnter2D (Collider2D other) {
        if (other.transform.CompareTag("DialogueWindow")) {
            talk = true;
            window = other;
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.transform.CompareTag("DialogueWindow")) {
            talk = false;
            window = null;
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine || !PhotonNetwork.IsConnected) {
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

                // debug dialogue key ew
                if (Input.GetKey("n")) {
                    GameObject.Find("GameManager").GetComponent<DialogueHandler>().NewDialogue(0, "testDialogue");
                }
            } else if (state == PlayerState.CurrentPlayerState.BATTLE) {
                // set the player to their battle station wherever that is
                rb.position = battleStation.position;
                rb.velocity = new Vector2(0f, 0f);
            } else if (state == PlayerState.CurrentPlayerState.INVENTORY) {
                rb.velocity = new Vector2(0f, 0f);
            } else if (state == PlayerState.CurrentPlayerState.DIALOGUE) {
                rb.velocity = new Vector2(0f, 0f);
            }
        }
    }

    void Update() {
        if (Input.GetButtonDown("AdvanceDialogue") && stateEnum.state == PlayerState.CurrentPlayerState.DIALOGUE) {
            GameObject.Find("GameManager").GetComponent<DialogueHandler>().NextLine();
        } else if (talk && Input.GetButtonDown("AdvanceDialogue")) {
            DialogueTrigger dialogue = window.transform.GetComponent<DialogueTrigger>();
            GameObject.Find("GameManager").GetComponent<DialogueHandler>().NewDialogue(dialogue.index, dialogue.file);
        }
    }
}
