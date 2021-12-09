using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public string typeOfUI;
    public GameObject attachedUI;
    PlayerState stateEnum;

    void Start() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState.CurrentPlayerState state = stateEnum.state;
        if (typeOfUI == "battle") {
            attachedUI.SetActive(state == PlayerState.CurrentPlayerState.BATTLE);
        }
    }
}
