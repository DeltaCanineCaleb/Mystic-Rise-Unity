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
    private string gameVersion = "1.0";

    private void Awake() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    private void Start() {
        characterMenu.SetActive(true);
    }

    private void OnConnectedToMaster () {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeCharacterInput () {
        race = RaceDropdownMenu.options[RaceDropdownMenu.value].text;
        if ((UsernameInput.text.Length >= 3 && UsernameInput.text.Length <= 16) && race != "Select...") {
            StartButton.GetComponent<Button>().interactable = true;
        } else {
            StartButton.GetComponent<Button>().interactable = false;
        }
    }

    public void SetCharacter () {
        characterMenu.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = UsernameInput.text + "/" + race;
    }

    public void CreateGame () {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {}, null);
    }

    public void JoinGame() {
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, new RoomOptions() {}, TypedLobby.Default);
    }

    private void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("MainGame");
    }
}