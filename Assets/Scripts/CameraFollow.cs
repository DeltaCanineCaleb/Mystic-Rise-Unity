using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D player;
    public Transform cameraTransform;
    PlayerState stateEnum;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    void LateUpdate()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (state == PlayerState.CurrentPlayerState.OVERWORLD)
            cameraTransform.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
