using System;
using UnityEngine;

namespace Levels.Music
{
    [Serializable]
    public class BeatObject
    {
        public float beatTime;
        
        public bool shouldDraw(BeatMap beatMap)
        {
            return Time.time + beatMap.screenTime - beatTime > 0;
        }
    }

    public enum BeatObjectGrade
    {
        Waiting,
        Excellent,
        Good,
        Okay,
        Miss,
        Bad,
        Inactive
    }
}