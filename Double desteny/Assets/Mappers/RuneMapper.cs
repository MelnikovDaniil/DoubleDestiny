using Assets.Runes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mappers
{
    public static class RuneMapper
    {
        public static RuneInfo FindCurrentByName(string name)
        {
            var runes = GetCurrentRunes();
            return runes.FirstOrDefault(x => x.name == name);
        }

        private static List<RuneInfo> GetCurrentRunes()
        {
            var runes = new List<RuneInfo>();
            var runeNames = GetCurrentRunesNames();
            for (int i = 0; i < runeNames.Count; i++)
            {
                var runeLevel = PlayerPrefs.GetInt($"Rune{runeNames[i]}");
                runes.Add(new RuneInfo
                {
                    name = runeNames[i],
                    level = runeLevel,
                });
            }
            return runes;
        }

        public static List<string> GetCurrentRunesNames()
        {
            var runes = new List<string>();
            var runesCount = PlayerPrefs.GetInt("RuneCurrentСount");
            for (int i = 0; i < runesCount; i++)
            {
                var runeName = PlayerPrefs.GetString($"RuneCurrent{i}");
                runes.Add(runeName);
            }

            return runes;
        }

        public static string GetCurrentRuneByIndex(int index)
        {
            return PlayerPrefs.GetString($"RuneCurrent{index}");
        }

        public static void SetRuneToCurrent(string name, int positionIndex)
        {
            PlayerPrefs.SetString($"RuneCurrent{ positionIndex }", name);
        }

        public static void DeleteCurrentRune(string name)
        {
            var names = GetCurrentRunesNames();
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == name)
                {
                    PlayerPrefs.SetString($"RuneCurrent{ i }", string.Empty);
                }
            }
        }

        public static void SetCurrentCount(int count)
        {
            PlayerPrefs.SetInt("RuneCurrentСount", count);
        }

        public static int GetRuneLevel(string name)
        {
            return PlayerPrefs.GetInt("Rune" + name);
        }

        public static void UpgradeLevel(string name)
        {
            PlayerPrefs.SetInt("Rune" + name, GetRuneLevel(name) + 1);
        }

        public static DateTime? GetRuneTime(string name)
        {
            var stringDate = PlayerPrefs.GetString($"Rune{name}Time");
            if (!string.IsNullOrEmpty(stringDate))
            {
                return DateTime.Parse(stringDate);
            }

            return null;
        }

        public static void SetRuneTime(string name, float minutes)
        {
            var date = DateTime.Now.ToUniversalTime().AddMinutes(minutes);
            PlayerPrefs.SetString($"Rune{name}Time", date.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        }

        public static void DisableTime(string name)
        {
            PlayerPrefs.DeleteKey($"Rune{name}Time");
        }

        public static int GetBoilersCount()
        {
            var boilers = PlayerPrefs.GetInt("Boilers");
            if (boilers > 0)
            {
                return boilers;
            }
            else
            {
                PlayerPrefs.SetInt("Boilers", 1);
                return 1;
            }
        }

        public static void AddBoiler()
        {
            PlayerPrefs.SetInt("Boilers", GetBoilersCount() + 1);
        }

        public static int GetCristallCount(CristallColorEnum cristallColor, CristallLevelEnum cristallLevel)
        {
            var color = CristallConstants.CristallNames[(int)cristallColor];
            var level = CristallConstants.CristallLevels[(int)cristallLevel];
            return PlayerPrefs.GetInt("Cristall" + color + level);
        }

        public static void PlusCristall(CristallColorEnum cristallColor, CristallLevelEnum cristallLevel, int value)
        {
            var color = CristallConstants.CristallNames[(int)cristallColor];
            var level = CristallConstants.CristallLevels[(int)cristallLevel];
            var cristallCount = PlayerPrefs.GetInt("Cristall" + color + level);
            PlayerPrefs.SetInt("Cristall" + color + level, cristallCount + value);
        }

        public static void MinusCristall(CristallColorEnum cristallColor, CristallLevelEnum cristallLevel, int value)
        {
            var color = CristallConstants.CristallNames[(int)cristallColor];
            var level = CristallConstants.CristallLevels[(int)cristallLevel];
            var cristallCount = PlayerPrefs.GetInt("Cristall" + color + level);
            PlayerPrefs.SetInt("Cristall" + color + level, cristallCount - value);
        }
    }
}
