using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node {

	public virtual Dialogue getDialogue()
    {
        return null;
    }

    public virtual Character GetCharacter()
    {
        return null;
    }

    public abstract bool hasDialogue();
    public abstract bool hasCharacter();
    public abstract bool isStart();
    public abstract bool isEnd();
    public abstract bool isQuestion();

}