using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractColliderManager : MonoBehaviour
{
    public List<State> stateList;

    public Transform interactCollider;
    public Transform fallbackLocation;
    public Animator animator;
    
    [Serializable]
    public class State
    {
        public List<String> stateNames;
        public Transform interactLocation;

        public bool tryToFitState(String animName,
            Transform interactCollider)
        {
            bool doesItFit = false;

            foreach (String name in stateNames)
            {
                doesItFit = animName.Equals(name);

                if (doesItFit)
                {
                    interactCollider.position = interactLocation.position;
                    break;
                }
            }

            return doesItFit;
        }
    }

    private void Update()
    {
        bool isSuccessful = false;
        
        /*
         * From https://forum.unity.com/threads/current-animator-state-name.331803/
         */
        AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        String animName = clipInfo.clip.name;
        
        foreach (State state in stateList)
        {
            isSuccessful = state.tryToFitState(animName, interactCollider);
            if (isSuccessful)
            {
                break;
            }
        }

        if (!isSuccessful)
        {
            interactCollider.position = fallbackLocation.position;
        }
    }
}
