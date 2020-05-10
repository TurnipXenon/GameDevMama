using System.Collections;
using System.Collections.Generic;
using Levels.Music;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/BeatMapData", order = 1)]
public class BeatMapData : ScriptableObject
{
    public List<int> timestampList;
    public BeatMap beatMapBase;

    public AudioClip GetAudioClip()
    {
        return beatMapBase.audioClip;
    }

    public BeatMap GenerateBeatMap()
    {
        BeatMap beatMap = beatMapBase.ShallowClone();
        
        beatMap.beatObjectList.Clear();

        foreach (int timestampInt in timestampList)
        {
            BeatObject beatObject = new BeatObject();
            beatObject.beatTime = timestampInt / 1000f;
            beatMap.beatObjectList.Add(beatObject);
        }
        
        return beatMap;
    }
}
