using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Runes
{
    public class CristallView : MonoBehaviour
    {
        public Image cristallImage;
        public Text currentCountText;
        public Button cristallButton;

        public CristallColorEnum cristallColor;
        public CristallLevelEnum cristallLevel;
    }
}
