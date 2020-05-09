using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public PlayerData playerData;

    private void Awake()
    {
        playerData.SetActiveDialogManager(this);
    }
}
