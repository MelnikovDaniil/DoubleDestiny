using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum BardEmotions
{
    Regular,
    Joy,
    Impression,
    VaultBoy,
    Awkwardness,
    Worry,
    StealthHehe,
    Cry,
    Pity,
    ManaFinger,
    SecondRegular,
    Cunning,
    Determination,
    Hmmmm,
    LittleSurprise,
    Minotaur,
    Witch,
    WitchEvent,
    LopFrame1,
    LopFrame2,
    LopFrame3,
    LopFrame4,
    ShowButton,
    FrogDeath,
    AngelFirst,
    AngelSecond,
    DevilFirst,
    DevilSecond,
}

public static class EnumExtentions
{
    public static String ConvertToString(this Enum eff)
    {
        return Enum.GetName(eff.GetType(), eff);
    }
}
