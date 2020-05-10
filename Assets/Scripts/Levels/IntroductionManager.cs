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

public class IntroductionManager : LevelManagerBase
{
    public float themeShowDuration = 140f;
    public GameObject prefabTheme;
    public GameObject canvas;
    public CodingMamaCharacter characterData;
    public DialogGroup beginningDialog;
    public DialogGroup resultDialog;

    private State _state;
    private DialogGroup _currentDialog;
    private IEnumerator _showImageCoroutine;

    private enum State
    {
        Beginning,
        Gameplay,
        Result
    }

    public override void Start()
    {
        base.Start();
        playerData.Restart();
        _state = State.Beginning;
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
                    StartCoroutine(ShowPictureCoroutine());
                }
                break;
            case State.Gameplay:
                break;
            case State.Result:
                result = _currentDialog.Display(playerData, characterData);

                if (result.isDone)
                {
                    SceneManager.LoadScene("Recruitment");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator ShowPictureCoroutine()
    {
        GameObject theme = Instantiate(prefabTheme, canvas.transform, true);

        yield return new WaitForSeconds(themeShowDuration);

        _state = State.Result;
        _currentDialog = resultDialog;
        _currentDialog.ApplyCharacter(characterData);
        _currentDialog.Display(playerData, characterData);
    }
}
