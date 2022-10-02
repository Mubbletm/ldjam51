using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    [SerializeField] private float shakeBufferTime = 3f;
    [SerializeField] private float confidenceBufferTime = 1f;
    [SerializeField] [Range(1f, 2f)] private float ySensitivity = 1f;
    [Space(10)]
    [SerializeField] [Range(10f, 80f)] private float movementSensitivity = 30f;
    [SerializeField] [Range(0f, 10f)] private float shakeSensitivity = 5f;

    private List<Vector2> rotationDeltas;
    private List<Vector4> confidenceTracker;

    List<ChoiceSubscription> subscribers = new List<ChoiceSubscription>();
    void Start()
    {
        rotationDeltas = new List<Vector2>();
        confidenceTracker = new List<Vector4>();
    }

    private void FixedUpdate()
    {
        if (subscribers.Count <= 0)
        {
            rotationDeltas.Clear();
            confidenceTracker.Clear();
            return;
        }
        
        if (rotationDeltas.Count > Mathf.RoundToInt(shakeBufferTime / Time.fixedDeltaTime))
        {
            rotationDeltas.RemoveAt(rotationDeltas.Count - 1);
        }

        Vector2 rotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotationDeltas.Insert(0, rotation);

        SHAKING shaking = shakeConfidence(rotationDeltas);
        if (shaking == SHAKING.NOT) return;
        BooleanUnityEvent cb = subscribers[0].callback;
        subscribers[0].unsubscribe();
        cb.Invoke(shaking == SHAKING.NOD);

        rotationDeltas.Clear();
        confidenceTracker.Clear();
    }

    SHAKING shakeConfidence(List<Vector2> movements)
    {
        Vector4 confidence = new Vector4();

        foreach(Vector2 movement in movements)
        {
            confidence.x += movement.x < 0 ? -movement.x : movement.x;
            confidence.y += movement.y < 0 ? -movement.y * ySensitivity : movement.y * ySensitivity;
            confidence.z += movement.x;
            confidence.w += movement.y / ySensitivity;
        }


        if (confidenceTracker.Count > Mathf.RoundToInt(confidenceBufferTime / Time.fixedDeltaTime))
        {
            confidenceTracker.RemoveAt(confidenceTracker.Count - 1);
        }
        confidenceTracker.Insert(0, confidence);

        Vector4 averageConfidence = new Vector4();
        foreach(Vector4 conf in confidenceTracker)
        {
            averageConfidence += conf;
        }
        averageConfidence /= confidenceTracker.Count;

        bool shake = isShake(averageConfidence.x, averageConfidence.z);
        bool nod = isShake(averageConfidence.y, averageConfidence.w);
        if (shake == nod) return SHAKING.NOT;
        if (shake) return SHAKING.SHAKE;
        return SHAKING.NOD;
    }

    private bool isShake(float movementConfidence, float mousePositionDelta)
    {
        return  (movementConfidence > movementSensitivity)
                &&
                (mousePositionDelta <= shakeSensitivity && -shakeSensitivity <= mousePositionDelta);
    }
    public bool hasSubscription(GameObject origin)
    {
        return subscribers.FindAll(o => o.subscriber.Equals(origin)).Count > 0;
    }

    public ChoiceSubscription GetSubscription(GameObject origin)
    {
        List<ChoiceSubscription> originSubscriptions = subscribers.FindAll(o => o.subscriber.Equals(origin));
        if (originSubscriptions.Count <= 0) throw new KeyNotFoundException("Couldn't find subscription with given origin");
        return originSubscriptions[0];
    }

    public ChoiceSubscription subscribe(GameObject origin, BooleanUnityEvent callback)
    {
        ChoiceSubscription subscription = new ChoiceSubscription(origin, callback, this);
        if (hasSubscription(origin)) GetSubscription(origin).unsubscribe();
        subscribers.Add(subscription);

        return subscription;
    }

    public void unsubscribe(ChoiceSubscription choiceSubscription)
    {
        subscribers.Remove(choiceSubscription);
    }

    enum SHAKING {
        NOT,
        SHAKE,
        NOD
    }

    public struct ChoiceSubscription
    {
        public GameObject subscriber;
        public BooleanUnityEvent callback;

        private ChoiceController choiceController;

        public ChoiceSubscription(GameObject subscriber, BooleanUnityEvent callback)
        {
            this.subscriber = subscriber;
            this.callback = callback;
            this.choiceController = null;
        }

        public ChoiceSubscription(GameObject subscriber, BooleanUnityEvent callback, ChoiceController choiceController)
        {
            this.subscriber = subscriber;
            this.callback = callback;
            this.choiceController = choiceController;
        }

        public void unsubscribe()
        {
            choiceController.unsubscribe(this);
        }
    }
}
