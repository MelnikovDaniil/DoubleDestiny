using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public virtual string Name { get; }
        public Sprite CharacterSprite;
        protected MinigameResult gameResult;

        public float volume;
        public AudioClip StartSound;
        public AudioClip EndSound;
        public AudioClip WinSound;
        public AudioClip LooseSound;

        private MinigameController MinigameController { get => GetComponentInParent<MinigameController>(); }

        public virtual void StopMinigame()
        {

        }

        public virtual void ContinueMinigame()
        {

        }

        public virtual void StartMinigame()
        {
            Time.timeScale = 0f;
            gameResult = MinigameResult.None;
            SoundManager.PlayMusic(Name + "Music");
            SoundManager.PlaySound(StartSound).SetPausable(false);
        }

        public virtual void EndMinigame()
        {
            Time.timeScale = 1f;
            if (gameResult == MinigameResult.Win)
            {
                MinigameController.WinCurrentEvent();
            }
            else if (gameResult == MinigameResult.Loose)
            {
                MinigameController.LooseCurrentEvent();
            }
            SoundManager.PlaySound(EndSound).SetPausable(false);
        }

        public virtual void WinMinigame()
        {
            SoundManager.PlaySound(WinSound).SetPausable(false);
        }

        public virtual void LooseMinigame()
        {
            SoundManager.PlaySound(LooseSound).SetPausable(false);
        }
    }
}
