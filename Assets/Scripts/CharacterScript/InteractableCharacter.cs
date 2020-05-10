using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    public CodingMamaCharacter characterData;

    private void Start()
    {
        characterData.RestartValues();
    }

    public Result Interact(PlayerData playerData)
    {
        return characterData.Interact(playerData);
    }

    public void Recruit(PlayerData playerData)
    {
        playerData.teamMemberList.Add(characterData);
        Destroy(gameObject);
    }
}
