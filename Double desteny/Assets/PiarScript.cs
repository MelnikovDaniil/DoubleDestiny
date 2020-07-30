using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PiarScript : MonoBehaviour
{
    public GameObject human;
    public Transform crowFront;
    public Transform crowBack;
    public List<Sprite> humansSprites;
    public Color backColor;
    public GameObject baka;

    private void Start()
    {
        var audioSource = SoundManager.PlaySound("ring")
            .SetVolume(0.3f)
            .AttachToObject(transform)
            .SetLooped()
            .SetPosition(transform.position)
            .Set3D(true)
            .Source;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 100.14f;
        audioSource.maxDistance = 101.5f;
        
        var ranFrontNumber = Random.Range(1, 6);
        var ranBackNumber = Random.Range((int)ranFrontNumber/2, ranFrontNumber);
        for (int i = 0; i < ranFrontNumber; i++)
        {
            GenerateHuman(crowFront, -85.0f, 55.0f);
        }
        for (int i = 0; i < ranBackNumber; i++)
        {
            var backHuman = GenerateHuman(crowBack, -65.0f, 40.0f);
            backHuman.GetComponent<Image>().color = backColor;
        }
    }

    private GameObject GenerateHuman(Transform transform, float minX, float maxX)
    {
        var generatedHuman = Instantiate(human, transform);
        var position = new Vector2(Random.Range(minX, maxX), Random.Range(-28.2f, -29.3f));
        generatedHuman.GetComponent<RectTransform>().anchoredPosition = position;
        generatedHuman.GetComponent<RectTransform>().localScale = new Vector3(-Mathf.Sign(position.x), 1, 1);
        generatedHuman.GetComponent<Image>().sprite = humansSprites[Random.Range(0, humansSprites.Count)];
        if (Random.Range(1, 10000) == 1)    
        {
            generatedHuman.GetComponent<RectTransform>().sizeDelta = new Vector2(baka.GetComponent<RectTransform>().rect.width, baka.GetComponent<RectTransform>().rect.height);
            generatedHuman.GetComponent<Image>().sprite = baka.GetComponent<Image>().sprite;
        }
        generatedHuman.GetComponent<Animator>().Play("Human", 0, Random.Range(0.0f,1.0f));
        return generatedHuman;
    }
}
