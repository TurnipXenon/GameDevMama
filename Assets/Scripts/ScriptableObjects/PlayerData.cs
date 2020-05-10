using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public String name;

    public DialogManager dialogManager;

    public EnumStage enumStage;

    public List<CodingMamaCharacter> teamMemberList = new List<CodingMamaCharacter>();

    public void SetActiveDialogManager(DialogManager dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public void SetEnumStage(EnumStage argEnumStage)
    {
        this.enumStage = argEnumStage;
    }

    public bool Recruit(PlayerData playerData, CodingMamaCharacter characterData)
    {
        teamMemberList.Add(characterData);
        return true;
    }
}
