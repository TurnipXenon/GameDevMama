using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using Levels.Coding;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelManagerCoding : LevelManagerBase
{
    public float gameplayDuration = 120f;
    public int minGenDuration = 2;
    public int maxGenDuration = 5;
    public Vector2 codingBugGenerationLimit = Vector2.one;

    public GameObject prefabBug;
    public TopDownCharacter2D playerAvatar;

    public DialogGroup beginningDialog;
    public DialogGroup resultDialog;

    private CodingMamaCharacter _characterData;
    private State _state;
    private DialogGroup _currentDialog;
    private IEnumerator _inputGenerationCoroutine;
    private float _genDuration;
    private float _startTime;
    private bool _isThreatened;

    private enum State
    {
        Beginning,
        Gameplay,
        Result
    }

    public override void Start()
    {
        base.Start();

        _genDuration = maxGenDuration;
        _state = State.Beginning;
        
        _characterData = playerData.GetRandomCharacter();
        _currentDialog = beginningDialog;
        _currentDialog.ApplyCharacter(_characterData);
        _currentDialog.Display(playerData, _characterData);
    }

    public void OnInteract()
    {
        Result result;
        switch (_state)
        {
            case State.Beginning:
                result = beginningDialog.Display(playerData, _characterData);

                if (result.isDone)
                {
                    _inputGenerationCoroutine = InputGenerationCoroutine();
                    _state = State.Gameplay;

                    StartCoroutine(_inputGenerationCoroutine);
                    StartCoroutine(GameplayTimer());
                }
                break;
            case State.Gameplay:
                break;
            case State.Result:
                result = _currentDialog.Display(playerData, _characterData);

                if (result.isDone)
                {
                    // todo: next scene
                    Debug.Log("Got to next scene");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMove(InputValue value)
    {
        if (_state == State.Gameplay)
        {
            playerAvatar.OnMove(value);
        }
    }

    IEnumerator InputGenerationCoroutine()
    {
        Vector2 randomLocation = new Vector2(
                UnityEngine.Random.Range(-codingBugGenerationLimit.x, codingBugGenerationLimit.x),
                UnityEngine.Random.Range(-codingBugGenerationLimit.y, codingBugGenerationLimit.y)
            );
        GameObject bugObject = Instantiate(prefabBug, randomLocation, Quaternion.identity);
        CodingBug script = bugObject.GetComponent<CodingBug>();
        script.SetData(codingBugGenerationLimit, 
            playerAvatar.transform, 
            _isThreatened);
        bugObject.transform.SetParent(transform);
        
        float randomTime = _random.Next(minGenDuration, maxGenDuration);
        float elapsedTime = Time.time - _startTime;

        if (elapsedTime > gameplayDuration * 0.8)
        {
            randomTime /= 4;
            _isThreatened = true;
        }
        else if (elapsedTime > gameplayDuration / 2)
        {
            randomTime /= 2;
        }
        
        yield return new WaitForSeconds(randomTime);

        _inputGenerationCoroutine = InputGenerationCoroutine();
        StartCoroutine(_inputGenerationCoroutine);
    }
    

    IEnumerator GameplayTimer()
    {
        _startTime = Time.time;
        yield return new WaitForSeconds(gameplayDuration);

        if (_inputGenerationCoroutine != null)
        {
            StopCoroutine(_inputGenerationCoroutine);
        }

        foreach (Transform child in transform)
        {
            CodingBug script = child.GetComponent<CodingBug>();

            if (script != null)
            {
                script.enabled = false;
            }
        }

        _state = State.Result;
        playerAvatar.Freeze();
        playerData.SetBugLeft(transform.childCount);
        
        _currentDialog = resultDialog;
        _currentDialog.ApplyCharacter(_characterData);
        _currentDialog.Display(playerData, _characterData);
    }
}
