using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D player;
    public Transform cameraTransform;
    public CurrentPlayerState state;

    // Update is called once per frame
    void Update()
    {
        if (state == CurrentPlayerState.OVERWORLD)
            cameraTransform.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
