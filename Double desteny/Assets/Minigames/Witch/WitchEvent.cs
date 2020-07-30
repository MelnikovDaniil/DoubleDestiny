using Assets.Mappers;
using Assets.Minigames;
using Assets.Minigames.EventCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchEvent : GameEvent
{
    
    public override string Name => MinigamesNames.Witch;

    public AudioClip eventStartSound;
    public AudioClip heheSound;
    public AudioClip anvilDropSound;


    [SerializeField]
    private Animator animator;// Start is called before the first frame update
    public override void StartEvent()
    {
        SoundManager.PlaySound(eventStartSound).SetVolume(volume);
        if (GuidMapper.GetStageStatus(GuidStages.Witch) == GuidStatus.NotActive)
        {
            GuidManager.Instance.ActivateStage(GuidStages.Witch);
            if (Mathf.Sign(transform.localScale.x) == -1)
            {
                transform.localScale *= new Vector2(-1, 1);
                transform.position *= new Vector2(-1, 1);
            }

        }
    }

    public override void WinCurrentEvent()
    {
        base.WinCurrentEvent();
        animator.Play("Witch_AnvilDrop", 0);
        CristalManager.SpawnCrystal(this, Assets.Runes.CristallLevelEnum.LowLevel, 2);
        Destroy(gameObject, 1.1f);
        SoundManager.PlaySound(anvilDropSound).SetVolume(volume);
    }

    public override void LooseCurrentEvent()
    {
        base.LooseCurrentEvent();
        animator.Play("Witch_Hehehe", 0);
        SoundManager.PlaySound(heheSound).SetVolume(volume);
        Destroy(gameObject, 2);
    }
}
