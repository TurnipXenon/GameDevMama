using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BehaviorCreateAudio : StateMachineBehaviour
{
    public GameObject prefabAudioManager;
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(prefabAudioManager);
    }
}
