using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Code from Brackeys.
 * Link: https://www.youtube.com/watch?time_continue=621&v=dwcT-Dch0bA&feature=emb_title
 */
public class PlatformCharacter2D : MonoBehaviour
{
    public float runSpeed = 40f;
    
    public PlatformerController2D controller;

    private float _horizontalMove = 0f;
    private bool _jump;

    public void OnMove(InputValue value)
    {
        _horizontalMove = value.Get<Vector2>().x * runSpeed;
    }

    public void OnJump()
    {
        _jump = true;
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }
}
