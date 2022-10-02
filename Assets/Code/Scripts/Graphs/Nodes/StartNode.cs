using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class StartNode : BaseNode
{
    [Output] public int start;

    public string characterName;
    public float defaultTalkingSpeed;
    public float defaultLoudness;
    public Color characterColor;

    public override Character GetCharacter()
    {
        return new Character(characterName, defaultTalkingSpeed, defaultLoudness, characterColor);
    }

    public override bool hasCharacter()
    {
        return true;
    }

    public override bool hasDialogue()
    {
        return false;
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
        return true;
    }
}