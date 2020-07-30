using Assets.Potions;
using UnityEngine;
using UnityEngine.UI;

public class PolPotion : MonoBehaviour
{
    public SpriteRenderer image;
    public Text text;
    public float radius;
    public float bubleCount = 0;
    public Vector2 generatedLocalPosition;
    public PotionInfo potionInfo;

    public bool potionFound = false;
    private Animator glowAnimator;

    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
        glowAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        ChekPotion();
    }

    public void ChekPotion()
    {
        bubleCount = 0;
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "buble")
            {
                bubleCount++;
            }
        }

        if (bubleCount == 0 && !potionFound)
        {
            FoundPotion();
        } 
    }

    public void FoundPotion()
    {
        potionFound = true;
        glowAnimator.SetTrigger("glow");
    }
}
