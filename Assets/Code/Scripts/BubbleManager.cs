using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject cameraControllerGameObject;

    private CameraController cameraController;

    void Start()
    {
        cameraController = cameraControllerGameObject.transform.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        ArrayList childPriorities = new ArrayList();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            TextBubble textBubble = child.GetComponent<TextBubble>();
            // childPriorities.Add(new ChildPriority(child, Mathf.RoundToInt(Vector3.Distance(player.transform.position, textBubble.origin.transform.position) * 100)));
            childPriorities.Add(new ChildPriority(child, Mathf.RoundToInt(Vector3.Distance(cameraController.getActiveCameraGameObject().transform.position, textBubble.origin.transform.position) * 100)));
        }

        childPriorities.Sort(new PriorityComparer());

        for (int i = 0; i < childPriorities.Count; i++)
        {
            ChildPriority childPriority = (ChildPriority) childPriorities[i];
            childPriority.child.SetSiblingIndex(i);
        }
    }

    public GameObject createBubble(Dialogue dialogue, GameObject origin)
    {
        GameObject newBubble = Instantiate(bubble, transform);
        TextBubble textBubble = newBubble.GetComponent<TextBubble>();
        textBubble.content = dialogue.content;
        textBubble.readTime = dialogue.readTime;
        textBubble.origin = origin;
        textBubble.loudness = dialogue.loudness;
        textBubble.timeOnScreen = dialogue.timeOnScreen;
        newBubble.name = "Bubble: " + dialogue.content;
        newBubble.SetActive(true);
        return newBubble;
    }

    private struct ChildPriority
    {
        public Transform child;
        public int priority;

        public ChildPriority(Transform child, int priority)
        {
            this.child = child;
            this.priority = priority;
        }
    }

    private class PriorityComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return ((ChildPriority)y).priority - ((ChildPriority)x).priority;
        }
    }
}
