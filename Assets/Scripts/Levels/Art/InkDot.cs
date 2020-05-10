using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Cinemachine;
using UnityEngine;

public class InkDot : MonoBehaviour
{
    private const float ORIGINAL_HALF_SIZE = 2f;

    public float borderGeneration = 11f;
    public float yBorder = 4.5f;
    public LevelManagerArt levelManagementArt;
    public GameObject prefabInkDot;

    private float _speed;
    private bool _generatedClone;
    private Vector2 _nextScale;
    private SpriteRenderer _spriteRenderer;
    private int _currentNum;

    private void Start()
    {
        _currentNum = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _generatedClone = false;
        _nextScale = levelManagementArt.GetRandomScale();
    }

    private void Update()
    {
        _speed = levelManagementArt.GetSpeed();
        if (_speed > 0)
        {
            transform.position = new Vector2(
                transform.position.x - (_speed * Time.deltaTime),
                transform.position.y);
        }

        if (!_generatedClone)
        {
            if (_spriteRenderer.bounds.max.x < borderGeneration + ORIGINAL_HALF_SIZE)
            {
                // randomize y here
                float quarterSizeY = ORIGINAL_HALF_SIZE * _nextScale.y;
                float randomY = transform.position.y
                                + UnityEngine.Random.Range(0, quarterSizeY)
                    - (quarterSizeY / 2f);
                randomY = Mathf.Clamp(randomY, -yBorder, yBorder);
                
                InkDot inkDotClone = Instantiate(
                        prefabInkDot, 
                        new Vector3(
                            _spriteRenderer.bounds.max.x,
                            randomY,
                            0
                        ), 
                        Quaternion.identity,
                        levelManagementArt.transform)
                    .GetComponent<InkDot>();
                _currentNum += 1;
                inkDotClone.SetData(levelManagementArt, _currentNum);
                inkDotClone.SetVariableSize(_nextScale);     
                _generatedClone = true;
            }
        }

        if (transform.position.x < -borderGeneration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TopDownCharacter2D>() != null)
        {
            levelManagementArt.InformEnterDot();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<TopDownCharacter2D>() != null)
        {
            levelManagementArt.InformExitDot();
        }
    }

    public void SetData(LevelManagerArt levelManagerArt, int currentNum)
    {
        _currentNum = currentNum;
        levelManagementArt = levelManagerArt;
        gameObject.name = "InkDot " + currentNum;
    }

    public void SetVariableSize(Vector2 localScale)
    {
        transform.localScale = localScale;
    }
}
