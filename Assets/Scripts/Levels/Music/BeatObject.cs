using System;
using UnityEngine;

namespace Levels.Music
{
    [Serializable]
    public class BeatObject
    {
        public float beatTime;
        public BeatObjectGrade grade = BeatObjectGrade.Waiting;

        public bool shouldDraw(BeatMap beatMap)
        {
            return beatMap.GetElapsedTime() + beatMap.screenTime - beatTime > 0;
        }

        public float GetLerpValue(BeatMap beatMap)
        {
            return (beatTime - beatMap.GetElapsedTime()) / beatMap.screenTime;
        }

        public void SetGrade(BeatObjectGrade argGrade)
        {
            grade = argGrade;
        }
    }

    public enum BeatObjectGrade
    {
        Waiting,
        Excellent,
        Good,
        Okay,
        Miss,
        Inactive
    }
}