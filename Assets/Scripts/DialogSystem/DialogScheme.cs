using System;
using System.Collections.Generic;
using ScriptableObjects;

namespace DialogSystem
{
    [Serializable]
    public abstract class DialogScheme
    {
        public EnumStage enumStage;
        protected List<DialogGroup> dialogGroupList = new List<DialogGroup>();
        
        [NonSerialized]
        protected int schemeIndex = 0;

        public void RestartValue()
        {
            RestartValueInit();
            
            schemeIndex = 0;
            
            foreach (DialogGroup dialogGroup in dialogGroupList)
            {
                dialogGroup.RestartValue();
            }
        }

        public virtual void RestartValueInit()
        {
            
        }

        public Result TryUsing(EnumStage ArgEnumStage, PlayerData playerData, CodingMamaCharacter characterData)
        {
            Result result = ResultFactory.CreateEndingResult();
            
            if (enumStage == ArgEnumStage)
            {
                // do stuff with dialog
                result = BodyTryUsing(playerData, characterData);
            }

            return result;
        }

        protected abstract Result BodyTryUsing(PlayerData playerData, CodingMamaCharacter characterData);
    }

    [Serializable]
    public class DialogGroup
    {
        public List<Dialog> dialogList = new List<Dialog>();
        
        [NonSerialized]
        protected int dialogIndex = 0;

        public void RestartValue()
        {
            dialogIndex = 0;
        }

        public Result Display(PlayerData playerData, CodingMamaCharacter characterData)
        {
            if (dialogIndex < dialogList.Count)
            {
                playerData.dialogManager.Show();
                playerData.dialogManager.DisplayDialog(dialogList[dialogIndex]);
                Result result = ResultFactory.CreateChattingResult();
                result.yesOrNoQuestion = dialogList[dialogIndex].yesOrNoResult;
                dialogIndex++;
                return result;
            }
            else
            {
                playerData.dialogManager.Hide();
                return ResultFactory.CreateEndingResult();
            }
        }
    }
}