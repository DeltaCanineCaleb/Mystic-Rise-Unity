using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject characterMenu;
    [SerializeField] private GameObject connectPanel; 
    [SerializeField] private GameObject loadingScreen;

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

    void Update()
    {
        if (Input.GetKey("b")) {
            race = "Dragonwolf";
            PhotonNetwork.LocalPlayer.NickName = "Hunter/" + race;
            loadingScreen.SetActive(true);
            loadingScreen.transform.GetChild(1).gameObject.SetActive(true);
            PhotonNetwork.CreateRoom("a", new RoomOptions() {}, null);
        }
    }

    public override void OnConnectedToMaster () {
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
        loadingScreen.SetActive(true);
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {}, TypedLobby.Default);
    }

    public void JoinGame() {
        loadingScreen.SetActive(true);
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, new RoomOptions() {}, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("MainGame");
    }
}