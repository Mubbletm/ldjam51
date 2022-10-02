using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected GameObject findChild(string name)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = findChild(gameObject.transform.GetChild(i).gameObject, name);
            if (child != null) return child;
        }

        return null;
    }

    protected GameObject findChild(GameObject otherGameObject, string name)
    {
        if (otherGameObject.name == name) return otherGameObject;
        if (otherGameObject.transform.childCount == 0) return null;
        for (int i = 0; i < otherGameObject.transform.childCount; i++)
        {
            GameObject child = findChild(otherGameObject.transform.GetChild(i).gameObject, name);
            if (child != null) return child;
        }
        return null;
    }
}
