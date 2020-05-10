using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScopeDialogItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI uiText;
    
    public void SetData(CodingMamaCharacter characterData, String dialog)
    {
        image.sprite = characterData.sprite;
        uiText.text = dialog;
    }
}
