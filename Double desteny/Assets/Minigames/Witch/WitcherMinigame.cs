using Assets.Mappers;
using Assets.Minigames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WitcherMinigame : Minigame
{
    public override string Name => MinigamesNames.Witch;

    public float potionsSpeed;
    public int potionsCount;
    public WitcherCharacter character;


    [SerializeField]
    private WitcherPotion potionPrefab;

    [SerializeField]
    private GameObject winText;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform potionTransform;

    private List<WitcherPotion> witcherPotions = new List<WitcherPotion>();
    private const int startPositonY = 200;
    private const int betweenPotionsY = 200;
    private const int betweenPotionsX = 150;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    // Start is called before the first frame update
    public override void StartMinigame()
    {
        base.StartMinigame();
        winText.SetActive(false);
        DestroyOldPotions();
        gameObject.SetActive(true);
        animator.Play("WitchMinigame", 0);
    }

    public void PlayAllPotions()
    {
        foreach (var item in witcherPotions)
        {
            if (item != null)
            {
                item.speed = potionsSpeed;
            }
        }
    }

    public override void StopMinigame()
    {
        base.StopMinigame();
        StopAllPotions();
        character.isStoped = true;
    }

    public override void ContinueMinigame()
    {
        base.ContinueMinigame();
        PlayAllPotions();
        character.isStoped = false;
    }

    public void StopAllPotions()
    {
        foreach (var item in witcherPotions)
        {
            if (item != null)
            {
                item.speed = 0;
            }
        }
    }

    private void DestroyOldPotions()
    {
        foreach (var item in witcherPotions)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        witcherPotions.Clear();
    }

    public void GeneratePotions()
    {
        character.SetupChar();
        WitcherPotion potionBuffer;
        int randomSpace;
        var sides = new List<int> { -1, 0, 1 };
        for (int i = 0; i < potionsCount; i++)
        {
            randomSpace = Random.Range(-1, 2);
            potionBuffer = Instantiate(potionPrefab, potionTransform);
            potionBuffer.speed = potionsSpeed;
            potionBuffer.rectTransform.anchoredPosition =
                new Vector2(
                    betweenPotionsX * sides.First(x => x != randomSpace),
                    startPositonY + betweenPotionsY * i);
            potionBuffer.witcherMinigame = this;
            witcherPotions.Add(potionBuffer);

            potionBuffer = Instantiate(potionPrefab, potionTransform);
            potionBuffer.speed = potionsSpeed;
            potionBuffer.rectTransform.anchoredPosition =
                new Vector2(
                    betweenPotionsX * sides.Last(x => x != randomSpace),
                    startPositonY + betweenPotionsY * i);
            potionBuffer.witcherMinigame = this;
            witcherPotions.Add(potionBuffer);
        }
        character.collider.enabled = true;
    }

    private void Update()
    {
        if (gameResult == MinigameResult.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = Input.mousePosition;

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    GuidManager.Instance.ChekSwipeSkip(Swipes.Left);
                    character.MoveLeft();
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    GuidManager.Instance.ChekSwipeSkip(Swipes.Right);
                    character.MoveRight();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                character.MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                character.MoveLeft();
            }

            if (witcherPotions.Any() && !witcherPotions.Any(x => x != null))
            {
                WinMinigame();
            }
        }
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        gameObject.SetActive(false);
    }

    public override void LooseMinigame()
    {
        base.LooseMinigame();
        character.CharacterDeath();
        StopAllPotions();
        gameResult = MinigameResult.Loose;
        animator.Play("WitchMinigame_WindowClose", 0);
    }

    public override void WinMinigame()
    {
        base.WinMinigame();
        gameResult = MinigameResult.Win;
        winText.SetActive(true);
        animator.Play("WitchMinigame_WindowClose", 0);
    }
}
