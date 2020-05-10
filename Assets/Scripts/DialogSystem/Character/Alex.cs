using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Character
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/Alex", order = 1)]
    public class Alex : CodingMamaCharacter
    {
        public RecruitmentDialogScheme recruitmentDialogScheme = new RecruitmentDialogScheme();

        private List<DialogScheme> dialogSchemeList = new List<DialogScheme>();

        [Serializable]
        public class RecruitmentDialogScheme : DialogScheme
        {
            public DialogGroup defaultLine = new DialogGroup();
            public DialogGroup introduction = new DialogGroup();
            public DialogGroup accepted = new DialogGroup();
            [NonSerialized]
            private State state = State.Introduction;
            [NonSerialized]
            private YesOrNoResult lastQuestion;

            private enum State
            {
                Introduction,
                Accepted,
                DefaultLine
            }
        
            protected override Result BodyTryUsing(PlayerData playerData, CodingMamaCharacter characterData)
            {
                DialogGroup currentDialog = defaultLine;
            
                switch (state)
                {
                    case State.Introduction:
                        currentDialog = introduction;
                        break;
                }
                
                Result result = currentDialog.Display(playerData, characterData);
                lastQuestion = result.yesOrNoQuestion;

                if (result.isDone)
                {
                    playerData.dialogManager.Hide();

                    switch (state)
                    {
                        case State.Accepted:
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
                    accepted,
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
                result = variable.TryUsing(playerData.enumStage, playerData, this);

                if (result.isAnActiveResponse)
                {
                    break;
                }
            }

            return result;
        }
    }
}