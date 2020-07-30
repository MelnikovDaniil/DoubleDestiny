using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Runes
{
    [Serializable]
    public class CraftingLevel
    {
        public CristallLevelEnum redCristallLevel;
        public int redCristallCount;

        public CristallLevelEnum greenCristallLevel;
        public int greenCristallCount;

        public CristallLevelEnum blueCristallLevel;
        public int blueCristallCount;

        public float craftingMinutes;
    }
}
