using System;
using ScriptableObjects;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public class Dialog
    {
        public Sprite sprite;
        public String name;
        [TextArea(3, 10)] public String sentences;
    
        public YesOrNoResult yesOrNoResult;

        public void ApplyCharacter(CodingMamaCharacter characterData)
        {
            sprite = characterData.sprite;
            name = characterData.name;
        }
    }
}
