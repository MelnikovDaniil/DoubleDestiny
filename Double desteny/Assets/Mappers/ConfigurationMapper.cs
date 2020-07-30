using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mappers
{
    public static class ConfigurationMapper
    {
        public static bool StartWithBoss 
        { 
            get => PlayerPrefs.GetInt("ConfigurationStartWithBoss", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("ConfigurationStartWithBoss", value ? 1 : 0);
        }
        public static bool RemoveMinigames
        {
            get => PlayerPrefs.GetInt("ConfigurationRemoveMinigames", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("ConfigurationRemoveMinigames", value ? 1 : 0);
        }
        public static float StepTime
        {
            get => PlayerPrefs.GetFloat("ConfigurationStepTime", 120);
            set => PlayerPrefs.SetFloat("ConfigurationStepTime", value);
        }
    }
}
