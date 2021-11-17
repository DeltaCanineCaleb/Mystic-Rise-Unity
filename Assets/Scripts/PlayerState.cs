using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentPlayerState { OVERWORLD, BATTLE }

public class PlayerState : MonoBehaviour
{
    public CurrentPlayerState state;

    void Start()
    {
        state = CurrentPlayerState.OVERWORLD;
    }
}
