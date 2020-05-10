using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using Levels.Coding;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelManagerArt : LevelManagerBase
{
    public float gameplayDuration = 120f;
    public float minDotSpeed = 3f;
    public float minScale = 0.4f;
    public float scaleAllowance = 0.6f;
    public float speedAllowance = 37f;

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
    private int _dotContactCount;
    private float _goodScore;
    private float _invLerpValue;
    private float _lerpValue = 1f;
    private float _badScore;

    private enum State
    {
        Beginning,
        Gameplay,
        Result
    }

    public override void Start()
    {
        base.Start();

        _state = State.Beginning;
        _invLerpValue = 1f;
        _lerpValue = 0f;
        
        _characterData = playerData.GetRandomCharacter();
        _currentDialog = beginningDialog;
        _currentDialog.ApplyCharacter(_characterData);
        _currentDialog.Display(playerData, _characterData);
    }

    private void Update()
    {
        if (_state == State.Gameplay)
        {
            _lerpValue = (Time.time - _startTime) / gameplayDuration;
            _invLerpValue = 1 - _lerpValue;
            
            if (_dotContactCount == 0)
            {
                _badScore += Time.deltaTime;
            }
            else
            {
                _goodScore += Time.deltaTime;
            }
        }
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
                    // _inputGenerationCoroutine = InputGenerationCoroutine();
                    _state = State.Gameplay;

                    // StartCoroutine(_inputGenerationCoroutine);
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

        playerData.SetArtScore(_goodScore, _badScore);
        
        _currentDialog = resultDialog;
        _currentDialog.ApplyCharacter(_characterData);
        _currentDialog.Display(playerData, _characterData);
    }

    public void InformEnterDot()
    {
        _dotContactCount++;
    }

    public void InformExitDot()
    {
        _dotContactCount--;
    }

    public float GetSpeed()
    {
        if (_state == State.Gameplay)
        {
            return minDotSpeed + speedAllowance * _lerpValue;
        }

        return -1;
    }

    public Vector3 GetRandomScale()
    {
        float randomFloat = UnityEngine.Random.Range(
            minScale,
            minScale + _invLerpValue * scaleAllowance
            );
        return new Vector2(randomFloat, randomFloat);
    }
}
