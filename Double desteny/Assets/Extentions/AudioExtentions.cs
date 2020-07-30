using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Extentions
{
    public static class AudioExtentions
    {
        public static void PlayWithRandomPitch(this AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(audioClip);
        }
    }
}
