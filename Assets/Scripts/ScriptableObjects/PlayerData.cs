using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public String name;

    public DialogManager dialogManager;

    public EnumStage enumStage;

    [Header("Variable end game results")] 
    public float scopeResult;
    public List<CodingMamaCharacter> teamMemberList;
    public int bugKillCount;
    public int remainingBugCount;
    
    private Random _random;

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

    public CodingMamaCharacter GetRandomCharacter()
    {
        if (_random == null)
        {
            _random = new Random();
        }

        int index = _random.Next(teamMemberList.Count);
        return teamMemberList[index];
    }

    public void SetScopeResult(float argScopeResult)
    {
        scopeResult = argScopeResult;
    }

    public void AddBugKill()
    {
        bugKillCount++;
    }
}
