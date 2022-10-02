using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class QuestionNode : BaseNode
{
    [Input] public int entry;
    [Output] public int yes;
    [Output] public int no;

    public string content;
    private float timeOnScreen = 10000;
    public float typingSpeed;
    public float loudness = .5f;


    public override Dialogue getDialogue()
    {
        return new Dialogue(content, timeOnScreen, timeOnScreen - typingSpeed, loudness);
    }

    public override bool hasCharacter()
    {
        return false;
    }

    public override bool hasDialogue()
    {
        return true;
    }

    public override bool isQuestion()
    {
        return true;
    }

    public override bool isStart()
    {
        return false;
    }

    public override bool isEnd()
    {
        return false;
    }
}