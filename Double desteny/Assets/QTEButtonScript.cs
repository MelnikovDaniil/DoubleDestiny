using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEButtonScript : MonoBehaviour
{
    private Image image;
    private float duration = 0.25f;
    private float currentDuration;
    private Color startColor;
    private float max1;
    private float max2;
    private float max3;
    private bool bl;

    private void Start()
    {
        image = GetComponent<Image>();
        currentDuration = duration;
        startColor = image.color;
        max1 = duration - duration / 3 * 2;
        max2 = duration - duration / 3 * 1;
    }

    private void Update()
    {
        

        if (currentDuration > duration / 3 * 2)
        {
            var persent = (currentDuration - duration / 3 * 2) / max1;
            image.color = Color.Lerp(Color.yellow, startColor, persent);
        }
        else if (currentDuration > duration / 3 * 1)
        {
            var persent = (currentDuration - duration / 3 * 1) / max2;
            image.color = Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.5f), Color.yellow, persent);
        }
        else
        {
            var persent = (currentDuration - duration / 3 * 0) / duration;
            image.color = Color.Lerp(Color.red, Color.Lerp(Color.red, Color.yellow, 0.5f), persent);
        }
        if (currentDuration < 0 && !bl)
        {
            GetComponent<Animator>().SetBool("failed", true);
            bl = true;
        }
        else
        {
            image.fillAmount = currentDuration / duration;

            currentDuration -= Time.deltaTime;
        }

    }
}
