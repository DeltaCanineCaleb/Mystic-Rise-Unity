using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class DialogueHandler : MonoBehaviour
{
    public GameObject textbox;
    public Text textboxText;
    public GameObject namebox;
    public Text nameboxText;
    public Text shopTextboxText;
    public Text shopNameboxText;
    public GameObject shop;
    public TextAsset[] dialogueFiles;

    PlayerState stateEnum;
    PlayerState.CurrentPlayerState playerState;

    string buyDialogue;
    string poorDialogue;
    [HideInInspector]
    public List<Item> shopStock = new List<Item>();

    TextAsset file;
    string dialogue;
    List<string> dialogueLines;
    string unparsedLine;
    int lineNumber;
    bool shopGUI;

    void Awake() {
        stateEnum = GameObject.Find("GameManager").GetComponent<PlayerState>();
    }
    
    Item AddItemToStock(string argument) {
        Item[] items = Resources.LoadAll<Item>("Items");
        foreach (var item in items) {
            if (item.name == argument) {
                return item;
            }
        }
        return null;
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
        for (int i = 0; i < dialogueLines.Count; i++) {
            dialogueLines[i] = dialogueLines[i].Trim();
        }
        textbox.SetActive(true);
        stateEnum.state = PlayerState.CurrentPlayerState.DIALOGUE;
        ReadLine(index);
        lineNumber = index;
    }

    public void ShopButton() {
        StartCoroutine(NextLine(true));
    }

    public IEnumerator NextLine(bool exitButton = false) {
        if (exitButton) {
            textbox.SetActive(true);
            shop.SetActive(false);
            shopGUI = false;
        }
        if (!shopGUI) {
            lineNumber += 1;
            if (dialogueLines[lineNumber] == "-END-") {
                textbox.SetActive(false);
                namebox.SetActive(false);
                stateEnum.state = PlayerState.CurrentPlayerState.OVERWORLD;
                yield break;
            } else if (dialogueLines[lineNumber] == "-SHOP-") {
                shopGUI = true;
                while (true) {
                    lineNumber += 1;
                    if (dialogueLines[lineNumber] == "-SHOP END-") {
                        break;
                    }
                    unparsedLine = dialogueLines[lineNumber];
                    String type = unparsedLine.Split(":"[0])[0];
                    String argument = unparsedLine.Split(":"[0])[1];
                    switch (type) {
                        case "shopkeep":
                            shopNameboxText.text = argument;
                            break;
                        case "openmessage":
                            shopTextboxText.text = argument;
                            break;
                        case "buymessage":
                            buyDialogue = argument;
                            break;
                        case "poormessage":
                            poorDialogue = argument;
                            break;
                        case "sell":
                            Item item = AddItemToStock(argument);
                            shopStock.Add(item);
                            break;
                        default:
                            // in case something breaks and nothing actually happens
                            Debug.Log("when the impostor is sus");
                            break;
                    }
                }
                GameObject.Find("Inventory + Dialogue UI").transform.GetChild(4).gameObject.GetComponent<InventoryUI>().UpdateUI();
                textbox.SetActive(false);
                namebox.SetActive(false);
                shop.SetActive(true);
            } else {
                ReadLine(lineNumber);
            }
        }
        yield break;
    }
}
