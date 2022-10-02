using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    GameObject wrapper;
    List<InteractionSubscription> subscribers = new List<InteractionSubscription>();

    void Start()
    {
        wrapper = transform.Find("Wrapper").gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && subscribers.Count > 0)
        {
            subscribers[0].callback.Invoke();
        }

        if (subscribers.Count > 0) wrapper.SetActive(true);
        else wrapper.SetActive(false);
    }

    public bool isVisible()
    {
        return wrapper.activeInHierarchy;
    }

    public bool hasSubscription(GameObject origin)
    {
        return subscribers.FindAll(o => o.subscriber.Equals(origin)).Count > 0;
    }

    public InteractionSubscription GetSubscription(GameObject origin)
    {
        List<InteractionSubscription> originSubscriptions = subscribers.FindAll(o => o.subscriber.Equals(origin));
        if (originSubscriptions.Count <= 0) throw new KeyNotFoundException("Couldn't find subscription with given origin");
        return originSubscriptions[0];
    }

    public InteractionSubscription subscribe(GameObject origin, UnityEvent callback)
    {
        if (hasSubscription(origin)) return GetSubscription(origin);
        InteractionSubscription subscription = new InteractionSubscription(origin, callback, this);
        subscribers.Add(subscription);
        return subscription;
    }

    public void unsubscribe(InteractionSubscription interactionSubscription)
    {
        subscribers.Remove(interactionSubscription);
    }

    public struct InteractionSubscription
    {
        public GameObject subscriber;
        public UnityEvent callback;

        private InteractController interactController;

        public InteractionSubscription(GameObject subscriber, UnityEvent callback)
        {
            this.subscriber = subscriber;
            this.callback = callback;
            this.interactController = null;
        }

        public InteractionSubscription(GameObject subscriber, UnityEvent callback, InteractController interactController)
        {
            this.subscriber = subscriber;
            this.callback = callback;
            this.interactController = interactController;
        }

        public void unsubscribe()
        {
            interactController.unsubscribe(this);
        }
    }
}
