using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * From Brackey's
 * Link: https://www.youtube.com/watch?time_continue=444&v=6OT43pvUyfY&feature=emb_title
 */
namespace DefaultNamespace
{
    public class AudioManager : MonoBehaviour
    {
        public List<AudioScriptable> persistentSoundList;

        public static AudioManager INSTANCE;

        private void Awake()
        {
            if (INSTANCE == null)
            {
                INSTANCE = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            foreach (AudioScriptable audioScriptable in persistentSoundList)
            {
                audioScriptable.SetSource(gameObject.AddComponent<AudioSource>());
                audioScriptable.TryPlay();
            }
        }

        public void Play(AudioScriptable audioScriptable)
        {
            audioScriptable.TryPlay();
        }
    }
}