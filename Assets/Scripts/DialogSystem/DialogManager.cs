using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogManager : MonoBehaviour
{
    public PlayerData playerData;
    public UIPanel uiPanel;

    private void Start()
    {
        playerData.SetActiveDialogManager(this);
    }

    public void Show()
    {
        uiPanel.Show();
    }

    public void DisplayDialog(Dialog dialog)
    {
        uiPanel.DisplayDialog(dialog);
    }

    public void Hide()
    {
        uiPanel.Hide();
    }
}
