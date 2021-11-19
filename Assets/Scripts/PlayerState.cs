using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public enum CurrentPlayerState { OVERWORLD, BATTLE }
    public CurrentPlayerState state = CurrentPlayerState.OVERWORLD;
}
