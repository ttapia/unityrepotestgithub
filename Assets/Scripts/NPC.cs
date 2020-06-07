using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string NPCName = "";
    public TextAsset[] dialogueTextFiles;

    private bool inRange = false;
    private bool inDialogue = false;
    private DialogueManager dialogueManager;
    private int dialogueIndex = 0;
    private DialogueSentence[][] dialogues;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        LoadNPCDialogues();
    }

    private void Update()
    {
        CheckForDialogue();
        if (inDialogue)
        {
            CheckIfDialogueEnded();
        }
    }

    private void CheckIfDialogueEnded()
    {
        if (!dialogueManager.inDialogue)
        {
            inDialogue = false;
            dialogueIndex++;
            if (dialogueIndex >= dialogues.Length)
            {
                dialogueIndex = 0;
            }
        }
    }

    private void CheckForDialogue()
    {
        if (inDialogue && Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.DisplayNextSentence();
        }
        else if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            inDialogue = true;
            dialogueManager.StartDialogue(dialogues[dialogueIndex], NPCName);
        }
    }

    private void LoadNPCDialogues()
    {
        if (dialogueTextFiles != null)
        {
            dialogues = new DialogueSentence[dialogueTextFiles.Length][];
            int i = 0;

            foreach (var dialogue in dialogueTextFiles)
            {
                var dialogueLines = dialogue.text.Split('\n');
                var dialogueSentences = new DialogueSentence[dialogueLines.Length];
                int j = 0;
                foreach (var dialogueLine in dialogueLines)
                {
                    var newDialogueSentence = DialogueSentence.CreateInstance(dialogueLine);
                    dialogueSentences[j] = newDialogueSentence;
                    j++;
                }

                dialogues[i] = dialogueSentences;
                i++;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inDialogue = false;
        inRange = false;
    }
}
