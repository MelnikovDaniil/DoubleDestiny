using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    Image image;
    public float cooldown = 0;
    public bool isCooldown;
    public bool heroDead;

    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        isCooldown = false;
        image = gameObject.transform.GetChild(1).GetComponent<Image>();
    }
    void Update()
    {
        if(isCooldown)
        {
            image.fillAmount += 1 / cooldown * Time.deltaTime;
            if(image.fillAmount>=1)
            {
                image.fillAmount = 1;
                isCooldown = false;
                if (!heroDead)
                {
                    if(cooldown != 0)
                    {
                        SoundManager.PlaySoundUI("cooldownFinished");
                    }
                    button.interactable = true;
                }
                GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void StartCooldown()
    {
        SoundManager.PlaySoundUI("button");
        isCooldown = true;
        image.fillAmount = 0;
        //GetComponent<Button>().enabled = false;
        button.interactable = false;
        GetComponent<Image>().color = Color.Lerp(Color.gray, Color.white, 0.5f);

    }
}
