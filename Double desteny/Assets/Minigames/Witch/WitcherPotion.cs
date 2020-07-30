using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitcherPotion : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public ParticleSystem glassParticles;
    public ParticleSystem poisonParticles;
    public float speed;
    public Collider2D collider;
    public WitcherMinigame witcherMinigame;
    public AudioClip potionSound;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += Vector2.down * speed;
        if (collider.enabled && rectTransform.anchoredPosition.y < -90)
        {
            DestroyPotion();
        }
    }

    public void DestroyPotion()
    {
        glassParticles.Play();
        poisonParticles.Play();
        image.enabled = false;
        collider.enabled = false;
        SoundManager.PlaySound(potionSound).SetVolume(witcherMinigame.volume).SetPausable(false);
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        //play your sound
        yield return new WaitForSecondsRealtime(1f); //waits 3 seconds
        Destroy(gameObject); //this will work after 3 seconds.
    }
}
