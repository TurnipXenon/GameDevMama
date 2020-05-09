using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CodingMamaCharacter : ScriptableObject
{
    public String name;
    public Sprite sprite;

    
    [NonSerialized]
    protected int index = 0;
    [NonSerialized]
    protected int state = 0;
    
    public abstract Result Interact(PlayerData playerData);

    public void RestartValues()
    {
        index = 0;
        state = 0;
    }
}