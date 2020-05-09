using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/HarCat", order = 1)]
public class HarCat : CodingMamaCharacter
{
    public List<Dialog> defaultLine = new List<Dialog>();
    public List<Dialog> introduction = new List<Dialog>();

    public override Result Interact(PlayerData playerData)
    {
        Result result = ResultFactory.CreateEndingResult();
        
        switch (playerData.enumStage)
        {
            case EnumStage.Recruitment:
                List<Dialog> currentDialog = defaultLine;
                Debug.Log("State: " + state);
                switch (state)
                {
                    case 0:
                        currentDialog = introduction;
                        break;
                }

                if (index < currentDialog.Count)
                {
                    playerData.dialogManager.Show();
                    playerData.dialogManager.DisplayDialog(currentDialog[index]);
                    result = ResultFactory.CreateChattingResult();
                    index++;
                }
                else
                {
                    result = ResultFactory.CreateEndingResult();
                    playerData.dialogManager.Hide();

                    switch (state)
                    {
                        case 0:
                            state = 1;
                            break;
                    }
                }
                
                break;
            
            default:
                Debug.LogWarning("State not yet implemented");
                break;
        }

        if (result.isDone)
        {
            index = 0;
        }

        return result;
    }
}