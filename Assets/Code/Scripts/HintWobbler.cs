using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintWobbler : MonoBehaviour
{
    public float rotationSpeed = 4;
    public float rotationIntensity = 2;
    public float positioningIntensity = 3;
    public float xSpeed = 3;
    public float ySpeed = 5;

    private RectTransform rectTransform;
    private float counter = 0;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
        originalRotation = rectTransform.rotation.eulerAngles;
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        Vector3 rotation = new Vector3(0, 0, Mathf.Sin(Time.time * rotationSpeed) * rotationIntensity);
        rectTransform.rotation = Quaternion.Euler(rotation + originalRotation);

        Vector3 position = new Vector3(
            Mathf.Sin(Time.time * xSpeed) * positioningIntensity, 
            Mathf.Cos(Time.time * ySpeed) * positioningIntensity,
            0
            );
        rectTransform.position = position + originalPosition;
        
    }
}
