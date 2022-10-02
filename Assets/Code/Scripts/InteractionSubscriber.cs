using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSubscriber : MonoBehaviour
{

    [SerializeField] private float viewingAngle = 30f;
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private UnityEvent interactionEvent = new UnityEvent();
    [Space(30)]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private InteractController interactController;

    private InteractController.InteractionSubscription subscription;
    private bool _interactionsEnabled = true;

    public bool interactionsEnabled
    {
        get { return _interactionsEnabled; }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_interactionsEnabled) return;
        if (isInInteractionRange())
        {
            subscription = interactController.subscribe(gameObject, interactionEvent);
        }
        else if (subscription.subscriber != null)
        {
            subscription.unsubscribe();
            subscription = new InteractController.InteractionSubscription(null, null);
        }
    }

    public void enableInteractions(bool enable)
    {
        _interactionsEnabled = enable;
        if (!enable && subscription.subscriber != null)
        {
            subscription.unsubscribe();
            subscription = new InteractController.InteractionSubscription(null, null);
        }
    }

    private void OnDestroy()
    {
        if (subscription.subscriber != null) subscription.unsubscribe();
    }

    public UnityEvent getInteractionEvent()
    {
        return this.interactionEvent;
    }

    bool isInInteractionRange()
    {
        return (interactionDistance >= Vector3.Distance(gameObject.transform.position, cameraController.getActiveCameraGameObject().transform.position)) &&
            (Vector3.Angle(cameraController.getActiveCameraGameObject().transform.forward, transform.position - cameraController.getActiveCameraGameObject().transform.position) < viewingAngle);
    }
}
