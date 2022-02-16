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
    public GameObject namebox;
    public Text nameboxText;
    public TextAsset[] dialogueFiles;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    TextAsset file;
    string dialogue;
    List<string> dialogueLines;
    string unparsedLine;
    int lineNumber;
    bool shopGUI

    void Awake() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }

    void ReadLine(int index) {
        unparsedLine = dialogueLines[index];
        if (unparsedLine.Contains(":")) {
            nameboxText.text = unparsedLine.Split(":"[0])[0];
            textboxText.text = unparsedLine.Split(":"[0])[1];
            namebox.SetActive(true);
        } else {
            textboxText.text = unparsedLine;
            namebox.SetActive(false);
        }
        
    }

    public void NewDialogue(int index, string textFile) {
        file = Array.Find(dialogueFiles, file => file.name == textFile);
        dialogue = file.text;
        dialogueLines = new List<string>();
        dialogueLines.AddRange(dialogue.Split("\n"[0]));

        textbox.SetActive(true);
        stateEnum.state = PlayerState.CurrentPlayerState.DIALOGUE;
        ReadLine(index);
        lineNumber = index;
    }

    public void NextLine() {
        lineNumber += 1;
        if (dialogueLines[lineNumber] == "-END-") {
            textbox.SetActive(false);
            namebox.SetActive(false);
            stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
        } else if (dialogueLines[lineNumber] == "-SHOP-") {
            // do the shop parsing thing
        } else {
            ReadLine(lineNumber);
        }
    }
}
