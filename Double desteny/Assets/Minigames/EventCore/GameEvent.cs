using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Minigames.EventCore
{
    public abstract class GameEvent : MonoBehaviour
    {
        public virtual string Name { get;}
        public MinigameController MinigameController { get; set; }

        public CristalManager CristalManager { get; set; }

        public float volume;

        public virtual void StartEvent()
        {

        }

        public virtual void StartMinigame()
        {
            MinigameController.StartMinigame(Name, this);
        }

        public virtual void WinCurrentEvent()
        {

        }

        public virtual void LooseCurrentEvent()
        {

        }
    }
}
