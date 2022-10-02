using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractionSubscriber))]
public class PickableObject : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;
    [Space(10)]
    [SerializeField] private UnityEvent onPickUp = new UnityEvent();
    [Space(30)]
    [SerializeField] private InventoryController inventoryController;
    private InteractionSubscriber interactionSubscriber;

    // Start is called before the first frame update
    void Start()
    {
        interactionSubscriber = GetComponent<InteractionSubscriber>();
        interactionSubscriber.getInteractionEvent().AddListener(pickup);
    }

    void pickup()
    {
        inventoryController.addItem(item, amount);
        onPickUp.Invoke();
        Destroy(gameObject);
    }

    public UnityEvent getOnPickUp()
    {
        return onPickUp;
    }
}
