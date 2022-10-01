using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBubble : MonoBehaviour
{
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        BubbleManager bubbleManager = GameObject.Find("Dialogue").GetComponent<BubbleManager>();
        bubbleManager.createBubble(dialogue, transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
