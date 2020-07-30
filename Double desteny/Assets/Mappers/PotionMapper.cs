using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mappers
{
    public static class PotionMapper
    {
        public static bool IsPoisonsReady()
        {
            var isReady = false;
            var stringDate = PlayerPrefs.GetString("PotionDate");

            if (!string.IsNullOrEmpty(stringDate))
            {
                var date = DateTime.Parse(stringDate);
                if (date < DateTime.Now.ToUniversalTime())
                {
                    isReady = true;
                }
            }

            return isReady;
        }

        public static void SetNextDate()
        {
            var date = DateTime.Today.AddDays(1).ToUniversalTime();
            PlayerPrefs.SetString("PotionDate", date.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        }

        public static int GetPotionsCount(string name)
        {
            return PlayerPrefs.GetInt("Potion" + name + "Count");
        }

        public static void MinusPotion(string name)
        {
            var count = PlayerPrefs.GetInt("Potion" + name + "Count");
            PlayerPrefs.SetInt("Potion" + name + "Count", count - 1);
        }

        public static void PlusPotion(string name)
        {
            var count = PlayerPrefs.GetInt("Potion" + name + "Count");
            PlayerPrefs.SetInt("Potion" + name + "Count", count + 1);
        }

        public static void SetIngredient(string name)
        {
            PlayerPrefs.SetString("PotionIngredient", name);
        }
        
        public static string GetIngredient()
        {
            return PlayerPrefs.GetString("PotionIngredient");
        }

        public static int GetGameToSnish()
        {
            return PlayerPrefs.GetInt("PotionSnich");
        }

        public static void AddGameToSnish()
        {
            PlayerPrefs.SetInt("PotionSnich", PlayerPrefs.GetInt("PotionSnich") + 1);
        }

        public static void RefreshSnich()
        {
            PlayerPrefs.SetInt("PotionSnich", 0);
        }
    }
}
