using Assets.Characters.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mappers
{

    public static class CharactersMapper
    {
        public static string GetCurrentRanger()
        {
            return PlayerPrefs.GetString("CurrentRanger", "Mage");
        }

        public static void SetCurrentRanger(this string characterName)
        {
            PlayerPrefs.SetString("CurrentRanger", characterName);
        }

        public static string GetCurrentWarrior()
        {
            return PlayerPrefs.GetString("CurrentWarrior", "Knight");
        }

        public static void SetCurrentWarrior(this string characterName)
        {
            PlayerPrefs.SetString("CurrentWarrior", characterName);
        }

        public static IEnumerable<Card> GetAvaliableCards(IEnumerable<Card> cards)
        {
            return cards.Where(x => x.character.Name.IsCharacterAvaliable());
        }

        public static bool IsCharacterAvaliable(this string charaterName)
        {
            return PlayerPrefs.GetInt(charaterName) == 1 ? true : false;
        }

        public static void EnableCharacter(this string characterName)
        {
            PlayerPrefs.SetInt(characterName, 1);
        }

        public static void CharacterStartup()
        {
            CharacterNames.Knight.EnableCharacter();
            CharacterNames.Mage.EnableCharacter();
            CharacterNames.Samurai.EnableCharacter();
            CharacterNames.Ninja.EnableCharacter();
            CharacterNames.Knight.SetCurrentWarrior();
            CharacterNames.Mage.SetCurrentRanger();
        }
    }
}
