using Assets.Interfaces;
using Assets.Mappers;
using Assets.Minigames;
using Assets.Minigames.EventCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurEvent : GameEvent, IUseCameraShake
{
    public override string Name => MinigamesNames.Minotaur;

    public CameraShake CameraShake { get; set; }

    public float speed;

    public AudioClip minotaurPunch;
    public AudioClip minotaurDeath;
    public AudioClip minotaurRun;

    private SMSound runSound;

    private bool minigameStarted;
    [SerializeField]
    private Animator animator;// Start is called before the first frame update
    private bool canPunch;

    public override void StartEvent()
    {
        runSound = SoundManager.PlaySound(minotaurRun)
            .SetVolume(volume)
            .SetLooped();

        if (GuidMapper.GetStageStatus(GuidStages.Minotaur) == GuidStatus.NotActive)
        {
            GuidManager.Instance.ActivateStage(GuidStages.Minotaur);
            if (Mathf.Sign(transform.localScale.x) == 1)
            {
                transform.localScale *= new Vector2(-1, 1);
                transform.position *= new Vector2(-1, 1);
            }

        }
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime * Mathf.Sign(transform.localScale.x);
        if (!minigameStarted && Mathf.Abs(transform.localPosition.x) <= 10)
        {
            minigameStarted = true;
            MinigameController.StartMinigame(Name, this);
        }
    }

    public override void WinCurrentEvent()
    {
        speed = 0;
        runSound.Stop();
        base.WinCurrentEvent();
        animator.Play("Minotaur_Death", 0);
        SoundManager.PlaySound(minotaurDeath).SetVolume(volume);
        Destroy(gameObject, 1.1f);
        CristalManager.SpawnCrystal(this, Assets.Runes.CristallLevelEnum.LowLevel, 2);
    }

    public override void LooseCurrentEvent()
    {
        base.LooseCurrentEvent();
        canPunch = true;
        Destroy(gameObject, 4f);
        StartCoroutine(StopSound());

    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(3.5f);
        runSound.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackZone" && canPunch)
        {
            animator.Play("Minotaur_Punch", 0);
            SoundManager.PlaySound(minotaurPunch).SetVolume(volume);
        }

    }

    public void ChameraShake()
    {
        StartCoroutine(CameraShake.Shake(0.3f, 0.15f));
    }
}
