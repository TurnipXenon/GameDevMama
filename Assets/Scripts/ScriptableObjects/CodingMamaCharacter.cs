using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character", order = 1)]
public class CodingMamaCharacter : ScriptableObject
{
    public String name;
    public Sprite sprite;

    public void Interact(PlayerData playerData)
    {
        // todo: do dialog shenanigans here
    }
}