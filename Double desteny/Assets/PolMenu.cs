using UnityEngine;
using Assets.Mappers;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using Assets.Potions;
using System.Collections.Generic;
using System.Linq;

public class PolMenu : MonoBehaviour
{
    public PolActivator polActivator;

    public float maxBubleX = 1000;
    public float maxBubleY = 450;
    public float maxSize = 200;
    public float minSize = 100;
    public float bublesCount = 1000;
    public float speed = 100;
    public float generationDelay = 3;

    public float maxPotionX = 500;
    public float maxPotionY = 250;
    public int minPotionCount = 1;
    public int maxPotionCount = 3;

    public GameObject bublePrefub;
    public PolPotion potionPrefab;
    public GameObject brush;

    public ParticleSystem polParticles;
    public Animator buttonAnimator;

    public List<PotionInfo> potions;
    public float movingTime = 1;

    public Transform potionContainer;


    private Vector3 pressPosition;
    private Vector2 secondPoint;
    private RectTransform _rectTransform;
    private Animator _animator;
    private Image _image;
    private Color color;

    private int potionsCount = 0;
    private float currentBlurTime = 0;
    private float currentMovingTime = 0;

    private bool disableBlur = false;
    private bool allPotionsFound = false;
    private List<PolPotion> potionViews;
    private List<GameObject> bubles;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
        potionViews = new List<PolPotion>();
        bubles = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            brush.SetActive(true);
            pressPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            //save ended touch 2d point
            pressPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               _rectTransform,
               pressPosition,
               GetComponentInParent<Canvas>().worldCamera,
               out secondPoint);
            brush.transform.localPosition = secondPoint;
        }

        if (Input.GetMouseButtonUp(0))
        {
            brush.SetActive(false);
        }

        if (!allPotionsFound && potionViews.Any() && potionViews.All(x => x.potionFound))
        {
            allPotionsFound = true;
            _animator.SetTrigger("ShowPotions");
            currentMovingTime = movingTime;
        }

        if (allPotionsFound && currentMovingTime > 0)
        {
            currentMovingTime -= Time.deltaTime;
            var coof = currentMovingTime / movingTime;

            for (int i = 0; i < potionViews.Count; i++)
            {
                var potionCoof = (float)(i + 1) / (float)(potionsCount + 1);
                var xPosition = Vector2.Lerp(new Vector2(-maxBubleX, 0), new Vector2(maxBubleX, 0), potionCoof);
                potionViews[i].transform.localPosition = Vector2.Lerp(xPosition, potionViews[i].generatedLocalPosition, coof);
                
            }

            if (currentMovingTime <= 0)
            {
                buttonAnimator.gameObject.SetActive(true);
                buttonAnimator.SetTrigger("ShowButton");
            }
        }

        if (disableBlur && currentBlurTime > 0)
        {
            currentBlurTime -= Time.deltaTime;
            var coof = (currentBlurTime/movingTime) * 7;
            _image.material.SetFloat("_Size", coof);
            _image.material.SetColor("_Color", Color.Lerp(Color.white, color, coof));
            if (currentBlurTime <= 0)
            {
                _animator.SetTrigger("HidePanel");
            }
        }
    }

    public void GetPotions()
    {
        foreach (var potion in potionViews)
        {
            PotionMapper.PlusPotion(potion.potionInfo.name);
        }
        PotionMapper.SetNextDate();
        polActivator.CheckPol();
        currentBlurTime = movingTime;
        disableBlur = true;
        //_animator.SetTrigger("HidePanel");
    }

    public void ClearAll()
    {
        foreach (var potion in potionViews)
        {
            Destroy(potion.gameObject);
        }
        potionViews.Clear();

        foreach (var buble in bubles)
        {
            Destroy(buble);
        }
        bubles.Clear();
        gameObject.SetActive(false);
    }

    public void ShowSmoke()
    {
        color = new Color(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f));
        GetComponent<Image>().enabled = false;
        buttonAnimator.GetComponent<Image>().color = color;
        var mainPart = polParticles.main;
        mainPart.startColor = color;
        polParticles.Play();
        gameObject.SetActive(true);
        StartCoroutine(GenerateBublesWithDelay());
        buttonAnimator.gameObject.SetActive(false);
        _image.material.SetFloat("_Size", 7);
        allPotionsFound = false;
        disableBlur = false;
    }



    public IEnumerator GenerateBublesWithDelay()
    {
        yield return new WaitForSeconds(generationDelay);
        _animator.SetTrigger("ShowPanel");
        polParticles.Stop();

        GetComponent<Image>().enabled = true;
        GetComponent<Image>().material.color = color;
        for (int i = 0; i < bublesCount; i++)
        {
            var buble = Instantiate(bublePrefub, transform);
            buble.transform.localPosition = new Vector3(Random.Range(-maxBubleX, maxBubleX),
                Random.Range(-maxBubleY, maxBubleY), 0);
            var scale = Random.Range(minSize, maxSize);
            buble.transform.localScale = new Vector3(scale, scale, scale);
            buble.GetComponent<SpriteRenderer>().color = color;
            bubles.Add(buble);
        }

        GaneratePotions();
    }

    private void GaneratePotions()
    {
        potionsCount = Random.Range(minPotionCount, maxPotionCount + 1);

        for (int i = 0; i < potionsCount; i++)
        {
            var randomPotion = potions[Random.Range(0, potions.Count)];
            var createdPotion = Instantiate(potionPrefab, potionContainer);
            
            createdPotion.image.sprite = randomPotion.sprite;
            createdPotion.text.text = randomPotion.name;

            createdPotion.transform.localPosition = new Vector3(Random.Range(-maxPotionX, maxPotionX),
                Random.Range(-maxPotionY, maxPotionY), 0);
            createdPotion.generatedLocalPosition = createdPotion.transform.localPosition;

            createdPotion.potionInfo = randomPotion;

            potionViews.Add(createdPotion);
        }
    }
}
