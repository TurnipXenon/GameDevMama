using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Character
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/HarCat", order = 1)]
    public class HarCat : CodingMamaCharacter
    {
        public RecruitmentDialogScheme recruitmentDialogScheme = new RecruitmentDialogScheme();

        private List<DialogScheme> dialogSchemeList = new List<DialogScheme>();

        [Serializable]
        public class RecruitmentDialogScheme : DialogScheme
        {
            public DialogGroup defaultLine = new DialogGroup();
            public DialogGroup introduction = new DialogGroup();
            [NonSerialized]
            private State state = State.Introduction;
        
            private enum State
            {
                Introduction,
                DefaultLine
            }
        
            protected override Result BodyTryUsing(PlayerData playerData)
            {
                DialogGroup currentDialog = defaultLine;
            
                switch (state)
                {
                    case State.Introduction:
                        currentDialog = introduction;
                        break;
                }
                
                Result result = currentDialog.Display(playerData);

                if (result.isDone)
                {
                    playerData.dialogManager.Hide();

                    switch (state)
                    {
                        case State.Introduction:
                            state = State.DefaultLine;
                            break;
                        default:
                            defaultLine.RestartValue();
                            break;
                    }
                }
                
                return result;
            }

            public override void RestartValueInit()
            {
                dialogGroupList = new List<DialogGroup>()
                {
                    defaultLine,
                    introduction
                };

                state = State.Introduction;
            }
        }
    
        public override void RestartValuesHook()
        {
            dialogSchemeList = new List<DialogScheme>()
            {
                recruitmentDialogScheme
            };

            foreach (var VARIABLE in dialogSchemeList)
            {
                VARIABLE.RestartValue();
            }
        }
        
        public override Result Interact(PlayerData playerData)
        {
            Result result = ResultFactory.CreateEndingResult();

            foreach (var variable in dialogSchemeList)
            {
                result = variable.TryUsing(playerData.enumStage, playerData);

                if (result.isAnActiveResponse)
                {
                    break;
                }
            }

            return result;
        }
    }
}