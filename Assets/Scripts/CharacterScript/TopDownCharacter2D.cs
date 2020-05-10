using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TopDownCharacter2D : MonoBehaviour
{
    private static int ANIM_PARAM_VECTOR_X = Animator.StringToHash("VectorX");
    private static int ANIM_PARAM_VECTOR_Y = Animator.StringToHash("VectorY");
    private static int ANIM_PARAM_IS_MOVING = Animator.StringToHash("IsMoving");
    
    public float walkSpeed = 5f;
    public float dialogEndDelay = 60f;
    
    public PlayerData playerData;
    
    public TopDownController2D controller;
    public Animator animator;
    public InteractionControlller interactionControlller;

    private Vector2 _moveValue = Vector2.zero;
    private bool _isMoving = false;
    private bool _allowMovement = true;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        if (_allowMovement)
        {
            _moveValue = value.Get<Vector2>();
        }
        else
        {
            _moveValue = Vector2.zero;
        }

        // region Animation
        _isMoving = _moveValue != Vector2.zero;

        if (_isMoving)
        {
            animator.SetFloat(ANIM_PARAM_VECTOR_X, _moveValue.x);
            animator.SetFloat(ANIM_PARAM_VECTOR_Y, _moveValue.y);
            
            if (_moveValue.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }

        animator.SetBool(ANIM_PARAM_IS_MOVING, _isMoving);
        // endregion Animation

    }

    public void OnInteract()
    {
        Result result = interactionControlller.Interact(playerData);
        _allowMovement = !result.isAnActiveResponse || result.isDone;
    }

    public void OnSecondary()
    {
        if (_allowMovement)
        {
            interactionControlller.Recruit(playerData);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(_moveValue * walkSpeed);
    }

    public void Freeze()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        animator.SetBool(ANIM_PARAM_IS_MOVING, false);
    }
}
