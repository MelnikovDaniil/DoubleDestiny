using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Runes
{
    public enum CristallLevelEnum
    {
        LowLevel,
        MiddleLevel,
        HighLevel,
    }

    public enum CristallColorEnum
    {
        Red,
        Green,
        Blue,
    }

    public class CristallConstants
    {
        public static readonly string[] CristallNames = { "Red", "Green", "Blue" };

        public static readonly string[] CristallLevels = { "Low", "Middle", "High" };
    }
}
