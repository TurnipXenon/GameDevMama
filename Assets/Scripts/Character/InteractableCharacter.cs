using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    public CodingMamaCharacter characterData;

    public void Interact(PlayerData playerData)
    {
        characterData.Interact(playerData);
    }
}
