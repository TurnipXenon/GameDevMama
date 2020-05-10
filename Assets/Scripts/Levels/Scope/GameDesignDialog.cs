using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class GameDesignDialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Sprite sprite;
    
    private CodingMamaCharacter _characterData;

    public void SetData(CodingMamaCharacter characterData)
    {
        _characterData = characterData;
        UpdateDialog();
    }

    private void UpdateDialog()
    {
        
    }
}
