using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptable", order = 1)]
public class AudioScriptable : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public void SetSource(AudioSource audioSource)
    {
        source = audioSource;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public void TryPlay()
    {
        if (source != null)
        {
            source.Play();
        }
    }

    public void TryPause()
    {
        if (source != null)
        {
            source.Pause();
        }
    }
}
