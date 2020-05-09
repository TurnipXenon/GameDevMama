using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    public CodingMamaCharacter characterData;

    public Result Interact(PlayerData playerData)
    {
        return characterData.Interact(playerData);
    }
}
