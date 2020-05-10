using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogSystem;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScope : LevelManagerBase
{
    public float gameplayDuration = 180f;
    public int minGenDuration = 2;
    public int maxGenDuration = 10;
    public int dialogCountLimit = 6;

    public EnumScope enumScopeUp;
    public EnumScope enumScopeDown;
    
    public BobController bobController;
    public DialogGroup beginningDialog;
    public DialogGroup overScopeDialog;
    public DialogGroup underScopeDialog;

    public GameObject prefabDialogItem;
    public Transform dialogHolder;
    
    private State _state;
    private CodingMamaCharacter _characterData;
    private IEnumerator _inputGenerationCoroutine;
    private List<GameObject> _dialogList;
    private float _startTime;
    private DialogGroup _resultDialog;

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
        _dialogList = new List<GameObject>();
        
        _characterData = playerData.GetRandomCharacter();
        beginningDialog.ApplyCharacter(_characterData);
        beginningDialog.Display(playerData, _characterData);
        bobController.GetComponent<Rigidbody2D>().isKinematic = true;
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
                    bobController.GetComponent<Rigidbody2D>().isKinematic = false;
                }
                break;
            case State.Gameplay:
                bobController.OnInteract();
                break;
            case State.Result:
                result = _resultDialog.Display(playerData, _characterData);

                if (result.isDone)
                {
                    SceneManager.LoadScene("Music");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnSecondary()
    {
        switch (_state)
        {
            case State.Beginning:
                break;
            case State.Gameplay:
                bobController.OnSecondary();
                break;
            case State.Result:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator InputGenerationCoroutine()
    {
        CodingMamaCharacter character = playerData.GetRandomCharacter();
        bool shouldScopeUp = bobController.transform.position.y > 0;
        if (_random.Next(10) > 6)
        {
            // contradict sometimes
            shouldScopeUp = !shouldScopeUp;
        }

        character.SetScopePreference(shouldScopeUp);
        
        GameObject dialogItem = Instantiate(prefabDialogItem, dialogHolder.parent);

        ScopeDialogItem script = dialogItem.GetComponent<ScopeDialogItem>();
        EnumScope.Dialog enumScopeDialog;
        float force;

        if (shouldScopeUp)
        {
            enumScopeDialog = enumScopeUp.GetDialog();
            force = enumScopeDialog.value;
        }
        else
        {
            enumScopeDialog = enumScopeDown.GetDialog();
            force = -enumScopeDialog.value;
        }
        
        script.SetData(character, enumScopeDialog.text);
        bobController.AddExternalForce(force);
        
        dialogItem.transform.SetParent(dialogHolder);
        _dialogList.Add(dialogItem);

        if (_dialogList.Count > dialogCountLimit)
        {
            Destroy(_dialogList[0]);
            _dialogList.RemoveAt(0);
        }
        
        float randomTime = _random.Next(minGenDuration, maxGenDuration);
        float elapsedTime = Time.time - _startTime;

        if (elapsedTime > (gameplayDuration * 0.8))
        {
            randomTime /= 4;
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
            bobController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _state = State.Result;

            DisplayResult();
        }
    }

    private void DisplayResult()
    {
        float scopeResult = bobController.transform.position.y;
        playerData.SetScopeResult(scopeResult);
        
        _resultDialog = scopeResult > 0 ? overScopeDialog : underScopeDialog;
        
        _characterData = playerData.GetRandomCharacter();
        _resultDialog.ApplyCharacter(_characterData);
        _resultDialog.Display(playerData, _characterData);
    }
}
