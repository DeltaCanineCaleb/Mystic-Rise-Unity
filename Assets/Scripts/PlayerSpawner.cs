using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Camera playerCamera;
    GameObject player;

    public Rigidbody2D SpawnPlayer() {
        player = Instantiate(PlayerPrefab, new Vector2(1f, -1f), Quaternion.identity);
        player.transform.GetChild(0).GetComponent<Canvas>().worldCamera = playerCamera;
        return player.GetComponent<Rigidbody2D>();
    }
}
