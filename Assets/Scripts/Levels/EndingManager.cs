using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using Levels.Coding;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : LevelManagerBase
{
    public float dialogCountLimit = 6;
    public float dialogListDelay = 1f;
    public CodingMamaCharacter characterData;
    public GameObject prefabDialogItem;
    public Transform dialogHolder;
    public DialogGroup beginningDialog;
    public DialogGroup resultDialog;
    public List<Dialog> creditList;
    
    private State _state;
    private DialogGroup _currentDialog;
    private List<Dialog> _generatedList;
    private List<GameObject> _dialogList;

    private enum State
    {
        Beginning,
        Gameplay,
        Result
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(GenerateDialogList());
        _dialogList = new List<GameObject>();
        _state = State.Beginning;
        
        
        characterData = playerData.GetRandomCharacter();
        _currentDialog = beginningDialog;
        _currentDialog.ApplyCharacter(characterData);
        _currentDialog.Display(playerData, characterData);
    }

    public void OnInteract()
    {
        Result result;
        switch (_state)
        {
            case State.Beginning:
                result = beginningDialog.Display(playerData, characterData);

                if (result.isDone)
                {
                    _state = State.Gameplay;
                    StartCoroutine(ShowDialogList());
                }
                break;
            case State.Gameplay:
                break;
            case State.Result:
                result = _currentDialog.Display(playerData, characterData);

                if (result.isDone)
                {
                    SceneManager.LoadScene("Startup");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator GenerateDialogList()
    {
        _generatedList = new List<Dialog>();
        _generatedList.Add(new Dialog("Aha! It's a trap! It's postmortem time!"));
        int recruitmentScore = 100;
        List<CodingMamaCharacter> memberList = playerData.teamMemberList;
        _generatedList.Add(new Dialog("Let's evaluate ourselves as our team first!"));
        
        if (memberList.Count < 3)
        {
            _generatedList.Add(new Dialog("Maybe we needed more people"));
            recruitmentScore -= factorial(3 - memberList.Count) * 10;
        }

        if (memberList.Count > 5)
        {
            recruitmentScore -= factorial(memberList.Count - 5) * 10;
        }

        foreach (GameRole role in Enum.GetValues(typeof(GameRole)))
        {
            int count = 0;

            foreach (CodingMamaCharacter member in memberList)
            {
                if (member.formalRole == role)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                recruitmentScore -= 20;
            }
        }
        
        _generatedList.Add(new Dialog("Our recruitment score is: " + recruitmentScore));

        if (recruitmentScore > 50)
        {
            _generatedList.Add(new Dialog("We got synergy! Can't wait to make games again!")); 
        }
        else
        {
            _generatedList.Add(new Dialog("At least we had fun making games together :)")); 
        }
        
        _generatedList.Add(new Dialog("can we talk about r scope tho?"));
        float scopeScore = 100 - (playerData.scopeResult * playerData.scopeResult);
        _generatedList.Add(new Dialog("Our scope score is: " + scopeScore));
        
        if (scopeScore > 50)
        {
            _generatedList.Add(new Dialog("I think we managed to make a game just right!")); 
        }
        else
        {
            _generatedList.Add(new Dialog("We should rethink how we scope our games...")); 
        }
        
        _generatedList.Add(new Dialog("How about the music?"));
        float musicScore = 100f * playerData.GetTotalMusicScore() / 54f;
        _generatedList.Add(new Dialog("Our music score is: " + musicScore));
        
        if (musicScore > 50)
        {
            _generatedList.Add(new Dialog("I guess we had a... JAM !!!")); 
        }
        else
        {
            _generatedList.Add(new Dialog("PAper jamn...")); 
        }
        
        _generatedList.Add(new Dialog("How about art? Eyes is the windows to the soul!"));
        float artScore = Mathf.Clamp(playerData.artScore, 0, 100);
        _generatedList.Add(new Dialog("Our art score is: " + artScore));

        if (artScore > 50)
        {
            _generatedList.Add(new Dialog("One step closer to Shweck 7!")); 
        }
        else
        {
            _generatedList.Add(new Dialog("Purghaps beaty beauty buty")); 
            _generatedList.Add(new Dialog("Yeah! That thing...")); 
            _generatedList.Add(new Dialog("It's in the eyes of the beholder.")); 
        }
        
        _generatedList.Add(new Dialog("We did not really get to test for bugs..."));
        float codingScore = Mathf.Clamp(playerData.bugKillCount - 5 * playerData.remainingBugCount,
            0, 100);
        _generatedList.Add(new Dialog("Our coding score is: " + codingScore));

        if (codingScore > 50)
        {
            _generatedList.Add(new Dialog("Ugh.. What are we even supposed to do?")); 
            _generatedList.Add(new Dialog("Our brains are too big.. Ugh..")); 
            _generatedList.Add(new Dialog("B I G  B R A I M")); 
        }
        else
        {
            _generatedList.Add(new Dialog("Hey! It works!")); 
        }
        
        _generatedList.Add(new Dialog("So overall, how did we do?"));
        float totalScore = (float) recruitmentScore + scopeScore + codingScore
                           + artScore + musicScore;
        _generatedList.Add(new Dialog("Our total score is: " + totalScore));

        if (codingScore > 50)
        {
            _generatedList.Add(new Dialog("Woah! Our game is fantastic!")); 
            _generatedList.Add(new Dialog("Woo!")); 
        }
        else
        {
            _generatedList.Add(new Dialog("We had fun! It's all about having fun :))")); 
        }
        
        _generatedList.AddRange(creditList);
        yield break;
    }

    IEnumerator ShowDialogList()
    {
        for (var i = 0; i < _generatedList.Count; i++)
        {
            GameObject dialogItem = Instantiate(prefabDialogItem, dialogHolder.parent);
            dialogItem.transform.SetParent(dialogHolder);
            ScopeDialogItem script = dialogItem.GetComponent<ScopeDialogItem>();
            script.SetData(playerData.GetRandomCharacter(), _generatedList[i].sentences);
            _dialogList.Add(dialogItem);

            if (_dialogList.Count > dialogCountLimit)
            {
                Destroy(_dialogList[0]);
                _dialogList.RemoveAt(0);
            }
            
            yield return new WaitForSeconds(dialogListDelay);
        }
        yield return new WaitForSeconds(dialogListDelay * 4);

        _state = State.Result;
        
        characterData = playerData.GetRandomCharacter();
        _currentDialog = resultDialog;
        _currentDialog.ApplyCharacter(characterData);
        _currentDialog.Display(playerData, characterData);
    }

    private int factorial(int num)
    {
        if (num == 0 || num == 1)
        {
            return 1;
        }
        else
        {
            return factorial(num - 1) * num;
        }
    }
}
