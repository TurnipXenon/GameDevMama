using System;
using System.Collections;
using System.Collections.Generic;
using Levels.Music;
using UnityEngine;

public class BeatRenderer : MonoBehaviour
{
    private static int ANIM_PARAM_DESTROY = Animator.StringToHash("Destroy");
    
    public float targetX = -6.5f;
    public float startX = 11f;
    public float limitY = 4f;
    
    private BeatObject _beatObject;
    private Vector2 _currentPosition;
    private Vector2 _targetPosition;
    private BeatMap _beatMap;
    private bool _isReady = false;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetData(BeatObject beatObject, BeatMap beatMap)
    {
        _beatMap = beatMap;
        _beatObject = beatObject;
    }

    public void Init()
    {
        _currentPosition = new Vector2(startX, UnityEngine.Random.Range(-limitY, limitY));
        _isReady = true;
    }

    private void Update()
    {
        if (_isReady)
        {
            float lerpValue = _beatObject.GetLerpValue(_beatMap);
            _currentPosition.x = Mathf.LerpUnclamped(targetX, startX,
                lerpValue);
            transform.position = _currentPosition;

            if (_beatObject.grade != BeatObjectGrade.Waiting)
            {
                Destroy(gameObject);
            }
        }
    }
}
