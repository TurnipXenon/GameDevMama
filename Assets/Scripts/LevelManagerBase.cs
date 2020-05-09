using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManagerBase : MonoBehaviour
{
    public PlayerData playerData;
    public EnumStage enumStage;

    private void Start()
    {
        playerData.SetEnumStage(enumStage);
    }
}
