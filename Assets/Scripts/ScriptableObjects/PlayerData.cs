using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public String name;

    public DialogManager dialogManager;

    public void SetActiveDialogManager(DialogManager dialogManager)
    {
        this.dialogManager = dialogManager;
    }
}
