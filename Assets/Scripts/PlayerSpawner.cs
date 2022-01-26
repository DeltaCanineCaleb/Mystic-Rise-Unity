using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{   
    public Camera playerCamera;
    [HideInInspector]
    public GameObject player;

    public GameObject SpawnPlayer(GameObject playerPrefab) {
        if (PhotonNetwork.IsConnected) {
            player = PhotonNetwork.Instantiate("Player", new Vector2(1f, -1f), Quaternion.identity, 0);
        } else {
            player = Instantiate(playerPrefab, new Vector2(1f, -1f), Quaternion.identity);
        }
        player.transform.GetChild(0).GetComponent<Canvas>().worldCamera = playerCamera;
        return player;
    }
}
