using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueHandler : MonoBehaviour
{
    public GameObject textbox;
    public Text textboxText;
    public TextAsset[] dialogueFiles;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    TextAsset file;
    string dialogue;
    List<string> dialogueLines;
    int lineNumber;

    void Awake() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    public void NewDialogue(int index, string textFile) {
        file = Array.Find(dialogueFiles, file => file.name == textFile);
        dialogue = file.text;
        dialogueLines = new List<string>();
        dialogueLines.AddRange(dialogue.Split("\n"[0]));

        textbox.SetActive(true);
        stateEnum.state = PlayerState.CurrentPlayerState.DIALOGUE;
        textboxText.text = dialogueLines[index-1];
        lineNumber = index-1;
    }

    public void NextLine() {
        lineNumber += 1;
        if (dialogueLines[lineNumber] != "-END-") {
            textboxText.text = dialogueLines[lineNumber];
        } else {
            textbox.SetActive(false);
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
        }
    }
}
