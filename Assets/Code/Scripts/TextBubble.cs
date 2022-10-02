using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
public class TextBubble : MonoBehaviour
{
    [SerializeField] public float timeOnScreen;        // How long the text bubble will be on screen. (s)
    [SerializeField] public float readTime;            // How much time the player has to read the bubble after the typewriter effect is over. (s)
    [SerializeField] public string content;            // The text inside the text bubble.
    [SerializeField] [Range(0f, 1.0f)] public float loudness;   // When the text is replaced with dots.
    [SerializeField] public Color backdropColor;
    [Space(10)]
    [SerializeField] public UnityEvent onDestroy;
    [Space(10)]
    [SerializeField] public UnityEvent onTimeout;
    [Space(30)]
    [SerializeField] private int bubbleBorderClearance;
    [SerializeField] private float timeout;
    [SerializeField] [Range(0.0f, 1.0f)] private float smallestSize;
    [SerializeField] private float smallestFontSize;
    [SerializeField] private float biggestFontSize;
    [SerializeField] [Range(1.0f, 10.0f)] private float resizeSensitivity;
    [SerializeField] [Range(0.0f, 10.0f)] private float smoothness = .5f;
    [Space(30)]
    [SerializeField] public GameObject origin;         // The gameobject that triggered the text bubble.
    [SerializeField] private CameraController cameraController;
    [SerializeField] private RectTransform UITransform;

    private TextMeshProUGUI contentTextMesh;
    private Image bubbleImage;
    private RectTransform rectTransform;
    private Vector3 originalBubbleScale;
    private float originalFontSize;
    private Vector3 oldPosition;
    private float typeSpeed;

    private bool timedOut = false;
    private int timeoutCounter = 0;

    void Start()
    {
        contentTextMesh = transform.Find("content").GetComponent<TextMeshProUGUI>();
        bubbleImage = transform.GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        originalBubbleScale = rectTransform.localScale;
        originalFontSize = contentTextMesh.fontSize;
        contentTextMesh.text = content;

        bubbleImage.color = backdropColor;

        if (biggestFontSize == 0) biggestFontSize = originalFontSize;

        StartCoroutine("type");
    }

    // Update is called once per frame
    void Update()
    {
        // Don't look at this ugliennes.
        // rectTransform.localScale = Vector3.Min(new Vector3(1,1, originalBubbleScale.z), Vector3.Max(new Vector3(smallestSize, smallestSize, originalBubbleScale.z), originalBubbleScale / (Vector3.Distance(camera.transform.position, origin.transform.position) / (10 * resizeSensitivity))));
        float distance = Vector3.Distance(
                    cameraController.getActiveCamera().transform.position,
                    origin.transform.position
                    );
        contentTextMesh.fontSize = Mathf.Min(biggestFontSize, Mathf.Max(smallestFontSize, originalFontSize * (resizeSensitivity / distance)));


        float scale = (rectTransform.localScale.x + rectTransform.localScale.y) / 2;

        if (distance / 30 >= loudness) contentTextMesh.text = "...";
        else contentTextMesh.text = content;

        Vector3 pos = cameraController.getActiveCamera().WorldToScreenPoint(origin.transform.position - new Vector3(0, 1.5f));
        pos.y = adjustPositionToBorder(UITransform.sizeDelta.y, rectTransform.sizeDelta.y, pos.y);
        pos.x = adjustPositionToBorder(UITransform.sizeDelta.x, rectTransform.sizeDelta.x, pos.x);

        if (oldPosition == null) oldPosition = pos;
        pos = Vector3.Lerp(oldPosition, pos, Time.deltaTime * smoothness);
        oldPosition = pos;
        rectTransform.position = pos;
    }

    private void FixedUpdate()
    {
        if (contentTextMesh.text == "...") timeoutCounter++;
        else timeoutCounter = 0;

        if (timeoutCounter > Mathf.RoundToInt(timeout / Time.fixedDeltaTime))
        {
            timedOut = true;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!timedOut) onDestroy.Invoke();
        else onTimeout.Invoke();
    }

    float adjustPositionToBorder(float sizeBorderSide, float sizeBubbleSide, float position)
    {
        return Mathf.Min(Mathf.Max(sizeBubbleSide, position), sizeBorderSide + sizeBubbleSide);


        //if (position > sizeBorderSide / 2) return Mathf.Min(sizeBorderSide - bubbleBorderClearance, position);
        //else return Mathf.Max(0 + sizeBubbleSide / 2.8f + bubbleBorderClearance, position);
    }

    IEnumerator type()
    {
        contentTextMesh.ForceMeshUpdate(true);
        int charactersLength = contentTextMesh.textInfo.characterCount;
        if (charactersLength <= 0) charactersLength = 1;
        contentTextMesh.maxVisibleCharacters = 0;
        typeSpeed = ((timeOnScreen - readTime) / charactersLength);

        int counter = 0;
        while (true)
        {
            int visibleCount = counter % (charactersLength + 1);
            contentTextMesh.maxVisibleCharacters = visibleCount;
            if (visibleCount >= charactersLength)
            {
                yield return new WaitForSeconds(readTime);
                Destroy(transform.gameObject);
                yield break;
            }
            counter++;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

}
