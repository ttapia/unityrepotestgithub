using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public GameObject dialogueOverlay;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Queue<DialogueSentence> sentences;
    public bool inDialogue = false;

    private float slowSpeed = 0.06f;
    private float fastSpeed = 0.01f;
    private float selectedSpeed = 0.06f;
    private bool sentenceFinished = true;
    private float speedUpTime = 0.15f;
    private Coroutine typingCoroutine = null;
    private Coroutine speedUpCoroutine = null;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
        dialogueOverlay.SetActive(false);
    }

    public void StartDialogue(DialogueSentence[] dialogue, string NPCName)
    {
        inDialogue = true;
        nameText.text = NPCName;

        sentences.Clear();

        foreach (var sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        Time.timeScale = 0;
        dialogueOverlay.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (!sentenceFinished)
        {
            speedUpCoroutine = StartCoroutine(SpeedUpTyping());
            return;
        }

        sentenceFinished = false;
        var sentence = sentences.Dequeue();

        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); }
        if (speedUpCoroutine != null)
        {
            selectedSpeed = slowSpeed;
            StopCoroutine(speedUpCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(sentence.sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (letter == '<')
            {
                dialogueText.text += "<font=\"Caffe Lungo Bold SDF\">";
                continue;
            }
            if (letter == '>')
            {
                dialogueText.text += "</font><font=\"Caffe Lungo SDF\">";
                continue;
            }
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(selectedSpeed);
        }
        sentenceFinished = true;
    }

    IEnumerator SpeedUpTyping()
    {
        selectedSpeed = fastSpeed;
        yield return new WaitForSecondsRealtime(speedUpTime);
        selectedSpeed = slowSpeed;
    }

    private void EndDialogue()
    {
        inDialogue = false;
        dialogueOverlay.SetActive(false);
        Time.timeScale = 1;
    }
}
