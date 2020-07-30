using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private EnemyScript target;
    public GameObject healthContainer;
    private Image healthBar;
    public GameObject filter;
    public Image health;

    public void ShowHealthBar(EnemyScript enemy)
    {
        target = enemy;
        healthContainer.SetActive(true);
        healthBar = health;
    }

    public void UpdateInfo()
    {
        var hpPercent = target.currentHP / target.HP;
        healthBar.fillAmount = hpPercent;
        healthBar.gameObject.GetComponent<Animator>().Play(0);
    }

    public void Lightning()
    {
        var color = Color.black;
        color.r = 0.06901923f;
        color.g = 0.0882495f;
        color.b = 0.3113208f;
        color.a = 0.3019608f;
        filter.GetComponent<Image>().color = color;
        filter.GetComponent<Animator>().Play("Lightning", 0);
        filter.GetComponent<AudioSource>().Play();
    }
}
