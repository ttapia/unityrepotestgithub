using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSentence : ScriptableObject
{
    public string sentence;
    public string textSpeed;
    public string textAnimation;
    public string characterAnimation;

    public void Init(string sentence, string speed, string textAnimation, string characterAnimation)
    {
        this.sentence = sentence;
        this.textSpeed = speed;
        this.textAnimation = textAnimation;
        this.characterAnimation = characterAnimation;
    }

    public new static DialogueSentence CreateInstance(string dialogueLine)
    {
        var sentence = dialogueLine.Split('/');
        var textSpeed = sentence[0];
        var textAnimation = sentence[1];
        var characterAnimation = sentence[2];
        var sentenceText = sentence[3];

        var data = ScriptableObject.CreateInstance<DialogueSentence>();
        data.Init(sentenceText, textSpeed, textAnimation, characterAnimation);
        return data;
    }
}
