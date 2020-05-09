using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/HarCat", order = 1)]
public class HarCat : CodingMamaCharacter
{

    public List<Dialog> introduction = new List<Dialog>();
    
    private int index = 0;
    
    public override Result Interact(PlayerData playerData)
    {
        Result result = ResultFactory.CreateEndingResult();
        
        switch (playerData.enumStage)
        {
            case EnumStage.Recruitment:

                if (index < introduction.Count)
                {
                    playerData.dialogManager.Show();
                    playerData.dialogManager.DisplayDialog(introduction[index]);
                    result = ResultFactory.CreateChattingResult();
                    index++;
                }
                else
                {
                    result = ResultFactory.CreateEndingResult();
                    playerData.dialogManager.Hide();
                    Debug.Log("Hide");
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