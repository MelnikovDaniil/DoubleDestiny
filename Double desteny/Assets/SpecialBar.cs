using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.AspectRatioFitter;

public class SpecialBar : MonoBehaviour
{
    public SpecialType specialType;
    public float maxValue;
    public Image charge;
    public Sprite skillIcon;
    public bool preserveAspect;
    public Vector2 size;
    private float coof;
    public bool useOutLine;
    public bool useMinDistanceBetween;
    public bool useAspectRationFitter;
    public AspectMode aspectMode;
    public float aspectCoof;
    public Vector2 outLineVector;
    public Color outlineColor;

    private bool isCreated;


    public void CreateSkillBar()
    {
        if (specialType == SpecialType.Integer)
        {
            //var skillpartObj = new GameObject();
            ////skillpartObj.SetActive(false);
            //var skillImage = skillpartObj.AddComponent<Image>();
            //skillImage.sprite = skillIcon;
            //skillImage.preserveAspect = true;

            for (int i = 0; i < maxValue; i++)
            {
                var skillpartObj = new GameObject();
                skillpartObj.SetActive(false);
                var skillImage = skillpartObj.AddComponent<Image>();
                skillpartObj.transform.parent = transform;
                skillImage.rectTransform.sizeDelta = size;
                skillImage.sprite = skillIcon;
                skillImage.preserveAspect = preserveAspect;
                if (useOutLine)
                {
                    var outline = skillpartObj.AddComponent<Outline>();
                    outline.effectColor = outlineColor;
                    outline.effectDistance = outLineVector;
                }
                if (useAspectRationFitter)
                {
                    var aspectRatioFitter = skillpartObj.AddComponent<AspectRatioFitter>();
                    aspectRatioFitter.aspectMode = aspectMode;
                    aspectRatioFitter.aspectRatio = aspectCoof;
                }
            }
        }
        isCreated = true;
    }

    public void UpdateSpecialBar(float value)
    {
        if (isCreated)
        {
            if (specialType == SpecialType.Procent)
            {
                coof = value / maxValue;
                charge.fillAmount = coof;
            }
            else if (specialType == SpecialType.Integer)
            {
                var integerValue = (int)Mathf.Round(value + 0.49f);
                for (int i = 0; i < (int)maxValue; i++)
                {
                    if (i < integerValue)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                    if (value % 1 != 0)
                        transform.GetChild(integerValue - 1).localScale = new Vector3(value % 1, value % 1, value % 1);
                }
            }
        }
    }

}
