using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EndNode : BaseNode
{
    [Input] public int end;

    public override bool hasCharacter()
    {
        return false;
    }

    public override bool hasDialogue()
    {
        return false;
    }

    public override bool isEnd()
    {
        return true;
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