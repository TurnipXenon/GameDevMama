using System;
using ScriptableObjects;
using UnityEngine;

namespace DialogSystem
{
    public abstract class YesOrNoResult : ScriptableObject
    {
        public abstract bool DoYes(PlayerData playerData, CodingMamaCharacter characterData);
        public abstract bool DoNo(PlayerData playerData, CodingMamaCharacter characterData);
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog/RecruitmentQuestion", order = 1)]
    public class RecruitmentAsk : YesOrNoResult
    {
        public override bool DoYes(PlayerData playerData, CodingMamaCharacter characterData)
        {
            return playerData.Recruit(playerData, characterData);
        }

        public override bool DoNo(PlayerData playerData, CodingMamaCharacter characterData)
        {
            return false;
        }
    }
}
