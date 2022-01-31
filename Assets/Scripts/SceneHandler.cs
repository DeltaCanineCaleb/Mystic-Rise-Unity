using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void BackToTitle () {
        SceneManager.LoadScene("StartMenu");
    }

    public void JoinARoom () {
        SceneManager.LoadScene("RoomJoining");
    }

    public void StartGame () {
        SceneManager.LoadScene("MainGame");
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Temo Village");
    }
}
