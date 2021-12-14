using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public enum CurrentPlayerState { OVERWORLD, BATTLE, INVENTORY }
    public CurrentPlayerState state = CurrentPlayerState.OVERWORLD;
}
