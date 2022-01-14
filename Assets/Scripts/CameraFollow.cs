using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D player;
    public Transform cameraTransform;
    PlayerState stateEnum;

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        stateEnum = gameManager.GetComponent<PlayerState>();
        player = gameManager.GetComponent<PlayerSpawner>().SpawnPlayer();
    }

    void LateUpdate()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD)
            cameraTransform.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
