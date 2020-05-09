using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCharacter2D : MonoBehaviour
{
    private static int ANIM_PARAM_VECTOR_X = Animator.StringToHash("VectorX");
    private static int ANIM_PARAM_VECTOR_Y = Animator.StringToHash("VectorY");
    private static int ANIM_PARAM_IS_MOVING = Animator.StringToHash("IsMoving");
    
    public float walkSpeed = 5f;

    public TopDownController2D controller;
    public Animator animator;

    private Vector2 _moveValue;
    private bool isMoving;

    public void OnMove(InputValue value)
    {
        _moveValue = value.Get<Vector2>();
        
        // region Animation
        isMoving = _moveValue != Vector2.zero;

        if (isMoving)
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
        else
        {
            
        }

        animator.SetBool(ANIM_PARAM_IS_MOVING, isMoving);
        // endregion Animation

    }

    public void OnInteract()
    {
        
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        controller.Move(_moveValue * walkSpeed);
    }
}
