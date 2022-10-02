using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public ConditionedDialogue[] dialogueConditions;
    [SerializeField] private ChoiceController choiceController;
    [SerializeField] private BubbleManager bubbleManager;

    private InteractionSubscriber interactionSubscriber;
    private Character character;
    private bool isTalking = false;

    private TextBubble lastQuestionBubble;


    void Start()
    {
        interactionSubscriber = GetComponent<InteractionSubscriber>();

        resetGraph();

        // Work with data.
        //_parser = StartCoroutine(ParseNode());
    }

    void Update()
    {
        if (interactionSubscriber == null) return;
        if (isTalking == interactionSubscriber.interactionsEnabled) interactionSubscriber.enableInteractions(!isTalking);
    }

    /*IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
    }*/

    public bool NextNode(string fieldName)
    {

        foreach (NodePort p in getCurrentGraph().current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                try { 
                    getCurrentGraph().current = p.Connection.node as BaseNode;
                    return true;
                } catch (System.Exception e)
                {
                    getCurrentConditionedDialogue().onFinish.Invoke();
                    resetGraph();
                    isTalking = false;
                    return false;
                }
            }
        }
        getCurrentConditionedDialogue().onFinish.Invoke();
        resetGraph();
        isTalking = false;
        return false;
    }

    public void onInteraction()
    {
        if (getCurrentGraph().current.isStart()) NextNode("start");
        else if (getCurrentGraph().current.isQuestion()) throw new System.Exception("Graph State shoudn't be possible. " + "{'content': " + getCurrentGraph().current.getDialogue().content + "}");
        else if (getCurrentGraph().current.hasDialogue()) NextNode("exit");


        isTalking = true;

        GameObject bubble = bubbleManager.createBubble(getCurrentGraph().current.getDialogue(), gameObject);
        TextBubble textBubble = bubble.GetComponent<TextBubble>();

        if (!getCurrentGraph().current.isQuestion())
        {
            textBubble.onDestroy.AddListener(onInteraction);
            textBubble.onTimeout.AddListener(resetGraph);
        } else
        {
            BooleanUnityEvent unityEvent = new BooleanUnityEvent();
            unityEvent.AddListener(answerQuestion);
            choiceController.subscribe(gameObject, unityEvent);
            lastQuestionBubble = textBubble;
        }
    }

    private void answerQuestion(bool answer)
    {
        NextNode(answer ? "yes" : "no");
        if (checkForEnd()) return;
        Destroy(lastQuestionBubble.gameObject);
        lastQuestionBubble = null;
        GameObject bubble = bubbleManager.createBubble(getCurrentGraph().current.getDialogue(), gameObject);
        TextBubble textBubble = bubble.GetComponent<TextBubble>();

        if (!getCurrentGraph().current.isQuestion())
        {
            textBubble.onDestroy.AddListener(onInteraction);
            textBubble.onTimeout.AddListener(resetGraph);
        }
        else
        {
            BooleanUnityEvent unityEvent = new BooleanUnityEvent();
            unityEvent.AddListener(answerQuestion);
            choiceController.subscribe(gameObject, unityEvent);
            lastQuestionBubble = textBubble;
        }
    }

    private bool checkForEnd()
    {
        if (getCurrentGraph().current.isEnd())
        {
            getCurrentConditionedDialogue().onFinish.Invoke();
            isTalking = false;
            resetGraph();
            return true;
        }
        return false;
    }

    public void resetGraph()
    {
        for (int i = 0; i < dialogueConditions.Length; i++) {
            DialogueGraph graph = dialogueConditions[i].dialogueGraph;
            foreach (BaseNode b in graph.nodes)
            {
                if (b.isStart())
                {
                    graph.current = b;
                    character = b.GetCharacter();
                    break;
                }
            }
        }
    }

    private DialogueGraph getCurrentGraph()
    {
        for (int i = 0; i < dialogueConditions.Length; i++)
        {
            if (dialogueConditions[i].condition.Invoke()) return dialogueConditions[i].dialogueGraph;
        }

        return null;
    }

    private ConditionedDialogue getCurrentConditionedDialogue()
    {
        for (int i = 0; i < dialogueConditions.Length; i++)
        {
            if (dialogueConditions[i].condition.Invoke()) return dialogueConditions[i];
        }

        return null;
    }

    public bool emptyCondition()
    {
        return true;
    }
}
