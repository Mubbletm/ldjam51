using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character", order = 1)]
public class Character : ScriptableObject
{
    public string name;
    public float defaultTalkingSpeed;
    public float defaultLoudness;
    public Color characterColor;

    public Character(string name, float defaultTalkingSpeed, float defaultLoudness, Color characterColor)
    {
        this.name = name;
        this.defaultTalkingSpeed = defaultTalkingSpeed;
        this.defaultLoudness = defaultLoudness;
        this.characterColor = characterColor;
    }
}
