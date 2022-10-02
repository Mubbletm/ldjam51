using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dialogue : ScriptableObject
{
    public string content;
    public float timeOnScreen;
    public float readTime;
    public float loudness;

    public Dialogue(string content, float timeOnScreen, float readTime, float loudness)
    {
        this.content = content;
        this.timeOnScreen = timeOnScreen;
        this.readTime = readTime;
        this.loudness = loudness;
    }
}
