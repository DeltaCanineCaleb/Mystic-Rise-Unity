using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    Rigidbody2D playerRB;
    public Transform cameraTransform;
    PlayerState stateEnum;

    void Awake() {
        GameObject gameManager = GameObject.Find("GameManager");
        stateEnum = gameManager.GetComponent<PlayerState>();
        player = gameManager.GetComponent<PlayerSpawner>().SpawnPlayer();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD)
            cameraTransform.transform.position = new Vector3(playerRB.transform.position.x, playerRB.transform.position.y, -10f);
    }
}
