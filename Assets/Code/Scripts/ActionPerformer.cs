using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPerformer : MonoBehaviour
{
    
    public UnityEvent cb;

    void Start()
    {
        cb.Invoke();
    }
}
