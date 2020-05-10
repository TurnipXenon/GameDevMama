using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "Scope", menuName = "ScriptableObjects/Enums/Scope", order = 1)]
public class EnumScope : ScriptableObject
{
    public List<Dialog> dialogList = new List<Dialog>();
    private Random _random = new Random();

    [Serializable]
    public class Dialog
    {
        [TextArea(2,10)]
        public String text = "Poggers";

        public int value = 1;
    }

    public Dialog GetDialog()
    {
        if (dialogList.Count > 0)
        {
            int index = _random.Next(dialogList.Count);
            return dialogList[index];
        }
        else
        {
            return new Dialog();
        }
    }
}