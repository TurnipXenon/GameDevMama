using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionControlller : MonoBehaviour
{
    private List<InteractableCharacter> _interactableCharacterList = new List<InteractableCharacter>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableCharacter character = other.GetComponent<InteractableCharacter>();
        if (character != null)
        {
            _interactableCharacterList.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableCharacter character = other.GetComponent<InteractableCharacter>();
        if (character != null && _interactableCharacterList.Count > 0)
        {
            _interactableCharacterList.RemoveAt(0);
        }
    }

    public Result Interact(PlayerData playerData)
    {
        if (_interactableCharacterList.Count > 0)
        {
            return _interactableCharacterList[_interactableCharacterList.Count - 1].Interact(playerData);
        }
        else
        {
            return ResultFactory.CreateNonReponseResult();
        }
    }

    public void Recruit(PlayerData playerData)
    {
        if (_interactableCharacterList.Count > 0)
        {
            _interactableCharacterList[_interactableCharacterList.Count - 1].Recruit(playerData);
        }
    }
}
