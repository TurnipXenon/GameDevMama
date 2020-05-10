using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Levels.Music
{
    [Serializable]
    public class BeatMap
    {
        public float excellentTolerance = 0.03f;
        public float goodTolerance = 0.06f;
        public float okayTolerance = 0.12f;
        public float screenTime = 3f;
        
        [HideInInspector]
        public List<BeatObject> beatObjectList;
        
        private int _reactIndex;
        private int _drawIndex;

        public BeatMap()
        {
            _reactIndex = 0;
            _drawIndex = 0;
        }

        public BeatObjectGrade React(bool activeReaction)
        {
            if (_reactIndex < beatObjectList.Count)
            {
                BeatObject currentBeat = beatObjectList[_reactIndex];
                float difference = currentBeat.beatTime - Time.time;

                if (difference > excellentTolerance)
                {
                    return BeatObjectGrade.Waiting;
                }

                float absDifference = Math.Abs(difference);

                if (activeReaction)
                {
                    if (absDifference < excellentTolerance)
                    {
                        return BeatObjectGrade.Excellent;
                    }
                    else if (absDifference < goodTolerance)
                    {
                        return BeatObjectGrade.Good;
                    }
                    else if (absDifference < okayTolerance)
                    {
                        return BeatObjectGrade.Okay;
                    }
                    else
                    {
                        return BeatObjectGrade.Miss;
                    }
                }
                else
                {
                    if (absDifference > okayTolerance)
                    {
                        return BeatObjectGrade.Miss;
                    }
                    else
                    {
                        return BeatObjectGrade.Waiting;
                    }
                }
            }
            else
            {
                return BeatObjectGrade.Inactive;
            }
        }

        public BeatObject GetDrawableBeat()
        {
            if (_drawIndex < beatObjectList.Count)
            {
                BeatObject beatObject = beatObjectList[_drawIndex];

                if (beatObject.shouldDraw(this))
                {
                    return beatObject;
                }
            }
            
            return null;
        }

        public BeatMap ShallowClone()
        {
            return (BeatMap)this.MemberwiseClone();
        }
    }
}