using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConditionedDialogue
{
    public DialogueGraph dialogueGraph;
    public Condition condition;
    public UnityEvent onFinish;

    [System.Serializable]
    public class Condition : SerializableCallback<bool>
    {

    }
}
