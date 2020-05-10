using System;
using System.Collections.Generic;
using DialogSystem;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class CodingMamaCharacter : ScriptableObject
    {
        public String name;
        public Sprite sprite;
        public String role;
        public GameRole formalRole;
        
        [SerializeField]
        private bool _scopePreference;

        public abstract Result Interact(PlayerData playerData);

        public void RestartValues()
        {
            RestartValuesHook();
        }

        public virtual void RestartValuesHook()
        {
        }

        public void SetScopePreference(bool scopeUp)
        {
            _scopePreference = scopeUp;
        }
    }

    public enum GameRole
    {
        Programmer,
        Artist,
        Writer,
        Musician
    }
}