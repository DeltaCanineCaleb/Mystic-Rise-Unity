using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject characterMenu;
    [SerializeField] private GameObject connectPanel; 

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private Dropdown RaceDropdownMenu;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject StartButton; 

    private string race;

    private void Awake() {
        PhotonNetwork.ConnectUsingSettings(); 
        
    }

    private void Start() {
        characterMenu.SetActive(true);
    }

    public void Update() {
        if (characterMenu.activeSelf == true) {
            race = RaceDropdownMenu.options[RaceDropdownMenu.value].text;
            if (UsernameInput.text.Length >= 3 && race != "Select...") {
                StartButton.GetComponent<Button>().interactable = true;
            } else {
                StartButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    private void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }
}