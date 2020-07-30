using UnityEngine;

namespace Assets.Mappers
{
    public static class MoneyMapper
    {
        public static int GetMoneyCount()
        {
            return PlayerPrefs.GetInt("Money");
        }

        public static void AddMoney(int money)
        {
            var currentMoney = PlayerPrefs.GetInt("Money");
            PlayerPrefs.SetInt("Money", currentMoney + money);
        }

        public static void RemoveMoney(int money)
        {
            var currentMoney = PlayerPrefs.GetInt("Money");
            PlayerPrefs.SetInt("Money", currentMoney - money);
        }

        public static void SetMoney(int money)
        {
            PlayerPrefs.SetInt("Money", money);
        }
    }
}
