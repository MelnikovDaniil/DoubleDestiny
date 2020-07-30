using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RGBPenka : MonoBehaviour
{
    public float duration;
    private int colorNumber;
    private float currentDuration;
    private float oneColorTime;
    public List<ParticleSystem> particles;
    public List<Color> colors;
    public Color currentColor;
    public bool isStaticColor;

    // Start is called before the first frame update
    void Start()
    {
        colorNumber = 1;
        oneColorTime = duration / colors.Count;
        currentDuration = duration;
        //currentColor = Color.white;
    }

    private void FixedUpdate()
    {
        if (!isStaticColor)
        {
            if (currentDuration > duration - colorNumber * oneColorTime && currentDuration > 0)
            {
                var procent = (currentDuration - (duration - colorNumber * oneColorTime)) / oneColorTime;
                currentColor = Color.Lerp(colors[colorNumber % colors.Count], colors[(colorNumber - 1) % colors.Count], procent);
            }
            else
            {
                colorNumber++;

                if (currentDuration < 0)
                {
                    colorNumber = 1;
                    currentDuration = duration;
                }
            }
            currentDuration -= Time.deltaTime;
        }

        foreach (var item in particles)
        {
            var c = item.main;
            c.startColor = currentColor;
        }
    }
    
}
