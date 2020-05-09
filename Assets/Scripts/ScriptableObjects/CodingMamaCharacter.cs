using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CodingMamaCharacter : ScriptableObject
{
    public String name;
    public Sprite sprite;

    public abstract Result Interact(PlayerData playerData);
}