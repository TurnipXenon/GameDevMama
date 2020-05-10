using System;
using System.Collections;
using System.Collections.Generic;
using Levels.Music;
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
    public float artScore;
    public List<BeatObjectGrade> musicGradeList;
    
    
    private Random _random;

    public void Restart()
    {
        scopeResult = 0f;
        teamMemberList = new List<CodingMamaCharacter>();
        musicGradeList = new List<BeatObjectGrade>();
    }

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

    public void SetBugLeft(int transformChildCount) {
        remainingBugCount = transformChildCount;
    }

    public void SetArtScore(float argGoodScore, float argBadScore)
    {
        artScore = argGoodScore + (argBadScore * 5);
    }

    public void SetMusicGrade(List<BeatObjectGrade> argGradeList)
    {
        musicGradeList = argGradeList;
    }

    public int GetTotalMusicScore()
    {
        int score = 0;
        foreach (var VARIABLE in musicGradeList)
        {
            switch (VARIABLE)
            {
                case BeatObjectGrade.Waiting:
                    break;
                case BeatObjectGrade.Excellent:
                    score += 6;
                    break;
                case BeatObjectGrade.Good:
                    score += 2;
                    break;
                case BeatObjectGrade.Okay:
                    score += 1;
                    break;
                case BeatObjectGrade.Miss:
                    score += 5;
                    break;
                case BeatObjectGrade.Inactive:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return score;
    }
}
