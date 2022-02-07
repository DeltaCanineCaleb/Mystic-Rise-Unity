using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public enum CurrentPlayerState { OVERWORLD, BATTLE, INVENTORY, DIALOGUE }
    public CurrentPlayerState state = CurrentPlayerState.OVERWORLD;
}
