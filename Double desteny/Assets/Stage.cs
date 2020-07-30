using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

namespace Assets
{
    [System.Serializable]
    public class Stage
    {
        [HideInInspector]
        public string name = "Stage";
        public List<CurrentMob> MobsList;
        public float Time;
        private int MobsCount;
        //private int GeneralCount;
        public bool IsActive;
        private float RateSpawn;
        public float CurrentRateSpawn;
        private float coefficient; //коэффициент для получения частоты спавна мобов
        private float currentTimeToSwap; //текущее время для смены частоты спавн
        private bool finalWave;
        System.Random random;

        public void Activate()
        {
            currentTimeToSwap = 0;
            coefficient = 1.4f;
            IsActive = true;
            random = new System.Random();
            CurrentMobs();
            finalWave = true;
        }
        public GameObject RandoMob()
        {
            currentTimeToSwap += CurrentRateSpawn;
            if (currentTimeToSwap >= Time / 5&&finalWave)
                NextRate();
            int num = random.Next(1, MobsCount);// от 0 и до MobsCount
            int sum = 0;
            bool bl = false;

            GameObject createdMob = null;

            foreach (CurrentMob item in MobsList)
            {
                sum += item.currentMob;
                if(num<=sum&&!bl)
                {
                    createdMob = item.GetMob();
                    MobsCount--;
                    if (MobsCount == 0)
                    {
                        if(finalWave)
                        {
                            finalWave = false;
                            foreach (CurrentMob elem in MobsList)
                            {
                                elem.currentMob = elem.waveMobs;
                                Time = Time * 0.3f;
                                coefficient = 1;
                                CurrentMobs();
                                //CurrentRateSpawn = 0.1f;// процентное соотношение, вручную
                            }
                        }
                        else IsActive = false;
                    }
                    bl = true;
                }
            }

            return createdMob;
        }
        public void CurrentMobs()
        {
            MobsCount = 0;
            foreach (CurrentMob item in MobsList)
            {
                MobsCount+=item.currentMob;
            }
            //GeneralCount = MobsCount;
            RateSpawn = Time / MobsCount;
            CurrentRateSpawn = RateSpawn * coefficient;
        }
        public void NextRate()
        {
            coefficient-=0.2f;
            CurrentRateSpawn = RateSpawn * coefficient;
            currentTimeToSwap = 0;
        }
    }
}
