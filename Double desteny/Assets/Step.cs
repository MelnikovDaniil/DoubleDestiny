using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    [System.Serializable]
    public class Step
    {
        [HideInInspector]
        public string name = "Step";
        public List<Stage> Stages;
        public Stage CurrentStage;
        public Sprite Bg;
        public bool IsActive;
        public bool IsBossFight;
        public GameObject Boss;


        public void StartStep()
        {
            IsActive = true;
            IsBossFight = false;
            Stages[0].Activate();
            DefineCurrentStage();
        }
        public void DefineCurrentStage()
        {
            Stage curStep = null;
            foreach (Stage item in Stages)
            {
                if (item.IsActive)
                    curStep = item;
            }
            CurrentStage = curStep;
        }
        public GameObject GoSpawnStage()
        {
            if (CurrentStage.IsActive == false)
            {
                NextStage();
                return null;
            }
            else
            {
                return CurrentStage.RandoMob();
            }
        }
        public void NextStage()
        {
            CurrentStage.IsActive = false;//count
            if (Stages.IndexOf(CurrentStage) + 1 < Stages.Count) //was <=
            {
                CurrentStage = Stages[Stages.IndexOf(CurrentStage) + 1];
                CurrentStage.Activate();
            }
            else
            {
                //IsActive = false
                IsBossFight = true;
                ActivateBossFight();
            };
        }

        public void ActivateBossFight()
        {
        }
    }
}
