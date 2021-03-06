﻿using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    private static int ANIM_PARAM_SHOW = Animator.StringToHash("Show");
    private static int ANIM_PARAM_HIDE = Animator.StringToHash("Hide");
    
    public Image image;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialog;
    public float characterDelay = 0.01f;
    
    public Animator animator;

    private String _sentences;
    private bool _isShown = false;

    public void Show()
    {
        if (_isShown)
        {
            return;
        }
        
        textDialog.text = "";
        animator.SetTrigger(ANIM_PARAM_SHOW);

        _isShown = true;
    }

    public void Hide()
    {
        if (!_isShown)
        {
            return;
        }
        
        StopAllCoroutines();
        textDialog.text = "";
        animator.SetTrigger(ANIM_PARAM_HIDE);

        _isShown = false;
    }

    public void DisplayDialog(Dialog dialog)
    {
        textName.text = dialog.name;
        image.sprite = dialog.sprite;

        _sentences = dialog.sentences;
        textDialog.text = "";
        
        StopAllCoroutines();
        StartCoroutine(ShowDialog());
    }

    IEnumerator ShowDialog()
    {
        foreach (char character in _sentences)
        {
            textDialog.text += character;
            yield return new WaitForSeconds(characterDelay);
        }
    }
}
