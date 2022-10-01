using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject createBubble(Dialogue dialogue, GameObject origin)
    {
        GameObject newBubble = Instantiate(bubble, transform);
        TextBubble textBubble = newBubble.GetComponent<TextBubble>();
        textBubble.content = dialogue.content;
        textBubble.readTime = dialogue.readTime;
        textBubble.priority = dialogue.priority;
        textBubble.origin = origin;
        textBubble.loudness = dialogue.loudness;
        textBubble.timeOnScreen = dialogue.timeOnScreen;
        newBubble.SetActive(true);
        return newBubble;
    }
}
