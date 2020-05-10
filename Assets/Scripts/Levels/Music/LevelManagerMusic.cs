using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DialogSystem;
using Levels.Coding;
using Levels.Music;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelManagerMusic : LevelManagerBase
{
    public float startDelay = 1.5f;
    
    public TopDownCharacter2D playerAvatar;
    public BeatMapData beatMapData;
    public GameObject prefabExcellent;
    public GameObject prefabGreat;
    public GameObject prefabGood;
    public GameObject prefabMiss;
    public GameObject prefabBeat;
    public AudioSource badAudio;
    public AudioSource goodAudio;
    
    public DialogGroup beginningDialog;
    public DialogGroup resultDialog;

    private CodingMamaCharacter _characterData;
    private State _state;
    private DialogGroup _currentDialog;
    private float _lerpValue = 1f;
    private List<BeatObjectGrade> gradeList;
    private AudioManager _audioManager;
    private BeatMap _beatMap;
    private AudioSource _audioSource;

    private enum State
    {
        Beginning,
        Gameplay,
        Result
    }

    public override void Start()
    {
        base.Start();
        
        gradeList = new List<BeatObjectGrade>();
        
        _audioManager = AudioManager.GetInstance();
        _beatMap = beatMapData.GenerateBeatMap();
        _state = State.Beginning;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = beatMapData.GetAudioClip();
        
        _characterData = playerData.GetRandomCharacter();
        _currentDialog = beginningDialog;
        _currentDialog.ApplyCharacter(_characterData);
        _currentDialog.Display(playerData, _characterData);
    }

    private void Update()
    {
        if (_state == State.Gameplay && _beatMap.IsReady())
        {
            if (_beatMap.IsDone())
            {
                _state = State.Result;
                playerData.SetMusicGrade(gradeList);

                _currentDialog = resultDialog;
                _currentDialog.ApplyCharacter(_characterData);
                _currentDialog.Display(playerData, _characterData);
                return;
            }

            BeatObject beatObject = _beatMap.RequestDrawableBeat();
            if (beatObject != null)
            {
                BeatRenderer beatRenderer = Instantiate(prefabBeat).GetComponent<BeatRenderer>();
                beatRenderer.SetData(beatObject, _beatMap);
                beatRenderer.Init();
            }
            
            BeatObjectGrade grade = _beatMap.React(false);
            
            if (grade == BeatObjectGrade.Miss)
            {
                gradeList.Add(grade);
                badAudio.Play();
                Instantiate(prefabMiss);
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
                    _state = State.Gameplay;
                    _audioManager.TryPause();
                    StartCoroutine(StarDelay());
                }
                break;
            case State.Gameplay:
                if (_beatMap.IsReady())
                {
                    BeatObjectGrade grade = _beatMap.React(true);
                    gradeList.Add(grade);
                    
                    switch (grade)
                    {
                        case BeatObjectGrade.Waiting:
                            break;
                        case BeatObjectGrade.Excellent:
                            Instantiate(prefabExcellent);
                            goodAudio.Play();
                            break;
                        case BeatObjectGrade.Good:
                            Instantiate(prefabGreat);
                            goodAudio.Play();
                            break;
                        case BeatObjectGrade.Okay:
                            Instantiate(prefabGood);
                            goodAudio.Play();
                            break;
                        case BeatObjectGrade.Miss:
                            break;
                        case BeatObjectGrade.Inactive:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
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

    IEnumerator StarDelay()
    {
        yield return new WaitForSeconds(startDelay);
        _audioSource.Play();
        _beatMap.Start(_audioSource);
    }
}
