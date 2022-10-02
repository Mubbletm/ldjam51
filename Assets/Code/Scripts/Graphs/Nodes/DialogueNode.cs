using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

    public string content;
    public float timeOnScreen;
    public float readTime;
    public float loudness = .5f;


    public override Dialogue getDialogue()
    {
        return new Dialogue(content, timeOnScreen, readTime, loudness);
    }

    public override bool hasCharacter()
    {
        return false;
    }

    public override bool hasDialogue()
    {
        return true;
    }

    public override bool isEnd()
    {
        return false;
    }

    public override bool isQuestion()
    {
        return false;
    }

    public override bool isStart()
    {
        return false;
    }
}