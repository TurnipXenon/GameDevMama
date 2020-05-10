using System;
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
    }
}
