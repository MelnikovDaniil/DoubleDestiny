using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Animator bardAnim;
    public GameObject songParticles;
    public Shop shop;
    public ScrollRect scrollRect;
    public Scrollbar bushScrollbar;
    public Scrollbar bgScrollbar;
    public bool enableSmoothMoving = true;

    private float parallaxValue = 0.0005f;

    private void Start()
    {
        foreach (var sound in SoundManager.Sounds)
        {
            sound.Stop();
        }
        SoundManager.PlayMusic("splin");
        SoundManager.PlaySound("menuBack").SetLooped(true);
        CharactersMapper.CharacterStartup();
        shop.GenerateCards();
        //BardStopMusicGoSleaping();
        if (SoundManager.GetMusicMuted())
        {
            bardAnim.Play("Ubard_Off",0,0);
            bardAnim.SetBool("chill", SoundManager.GetMusicMuted());
        }
    }

    private void Update()
    {
        if (enableSmoothMoving)
        {
            scrollRect.horizontalNormalizedPosition += parallaxValue;
            if (scrollRect.horizontalNormalizedPosition > 0.99f || scrollRect.horizontalNormalizedPosition < 0.01f)
            {
                parallaxValue *= -1;
            }
            bushScrollbar.value = scrollRect.horizontalNormalizedPosition;
            bgScrollbar.value = scrollRect.horizontalNormalizedPosition;
        }
    }

    public void BardChill()
    {
        bardAnim.SetBool("chill", !SoundManager.GetMusicMuted());
    }
    public void BardStopMusicGoSleaping()
    {
        SoundManager.SetMusicMuted(!SoundManager.GetMusicMuted());
        if (SoundManager.GetMusicMuted())
        {
            songParticles.GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            songParticles.GetComponent<ParticleSystem>().Play();
        }
        
    }

    public void Play()
    {
        SceneManager.LoadScene("game");
    }
}
