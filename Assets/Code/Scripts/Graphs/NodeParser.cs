using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    private Character character;

    Coroutine _parser;

    void Start()
    {
        foreach(BaseNode b in graph.nodes)
        {
            if (b.isStart())
            {
                graph.current = b;
                character = b.GetCharacter();
                break;
            }
        }

        // Work with data.
        //_parser = StartCoroutine(ParseNode());
    }

    /*IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
    }*/

    public void NextNode(string fieldName)
    {
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }

        foreach(NodePort p in graph.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }

        // Work with data.
        //StartCoroutine("ParseNode");
    }
}
