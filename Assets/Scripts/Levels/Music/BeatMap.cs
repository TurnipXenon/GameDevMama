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
        public AudioClip audioClip;
        
        [HideInInspector]
        public List<BeatObject> beatObjectList;
        
        private int _reactIndex;
        private int _drawIndex;
        private AudioSource _audioSource;

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
                float difference = currentBeat.beatTime - _audioSource.time;

                if (difference > okayTolerance)
                {
                    return BeatObjectGrade.Waiting;
                }

                float absDifference = Math.Abs(difference);
                BeatObjectGrade grade;
                
                if (activeReaction)
                {
                    if (absDifference < excellentTolerance)
                    {
                        grade = BeatObjectGrade.Excellent;
                    }
                    else if (absDifference < goodTolerance)
                    {
                        grade = BeatObjectGrade.Good;
                    }
                    else if (absDifference < okayTolerance)
                    {
                        grade = BeatObjectGrade.Okay;
                    }
                    else
                    {
                        grade = BeatObjectGrade.Miss;
                    }
                }
                else
                {
                    if (absDifference > okayTolerance)
                    {
                        grade = BeatObjectGrade.Miss;
                    }
                    else
                    {
                        grade = BeatObjectGrade.Waiting;
                    }
                }

                if (grade != BeatObjectGrade.Waiting)
                {
                    _reactIndex++;
                }
                
                currentBeat.SetGrade(grade);

                return grade;
            }
            else
            {
                return BeatObjectGrade.Inactive;
            }
        }

        public BeatObject RequestDrawableBeat()
        {
            if (_drawIndex < beatObjectList.Count)
            {
                BeatObject beatObject = beatObjectList[_drawIndex];

                if (beatObject.shouldDraw(this))
                {
                    _drawIndex++;
                    return beatObject;
                }
            }
            
            return null;
        }

        public BeatMap ShallowClone()
        {
            return (BeatMap)this.MemberwiseClone();
        }

        public void Start(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public bool IsReady()
        {
            return _audioSource != null;
        }

        public bool IsDone()
        {
            return !_audioSource.isPlaying;
        }

        public float GetElapsedTime()
        {
            return _audioSource.time;
        }
    }
}