using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mappers
{
    public enum GuidStatus
    {
        NotActive,
        WhaitingForActivation,
        Done,
    }

    public enum GuidStages
    {
        Initial,
        Death,
        Shop,
        Special,
        Skill,
        SkillSecondStage,
        PassiveSkill,
        Ulti,
        ChoosingWay,
        BaseMinigame,
        Witch,
        Minotaur,
        Runes,
    }

    public static class GuidMapper
    {
        public static GuidStatus GetStageStatus(GuidStages stage)
        {
            var statusNum = PlayerPrefs.GetInt($"Guid{stage.ConvertToString()}");
            return (GuidStatus)statusNum;
        }

        public static void SetStageStatus(GuidStages stage, GuidStatus status)
        {
           PlayerPrefs.SetInt($"Guid{stage.ConvertToString()}", (int)status);
        }
    }
}
