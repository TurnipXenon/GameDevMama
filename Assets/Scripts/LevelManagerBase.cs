using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class LevelManagerBase : MonoBehaviour
{
    public PlayerData playerData;
    public EnumStage enumStage;

    protected Random _random;
    
    public virtual void Start()
    {
        playerData.SetEnumStage(enumStage);
        _random = new Random();
    }
}
