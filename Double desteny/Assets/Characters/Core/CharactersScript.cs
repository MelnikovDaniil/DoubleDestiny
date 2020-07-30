using Assets.BuffSystem;
using Assets.Interfaces;
using Assets.Mappers;
using Assets.Runes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CharactersScript : MonoBehaviour
{
    public static int points;
    public Text scoreText;
    public bool wariorBl, mageBl;
    public GameObject warior, ranger;
    public Color swapColor;
    public Button rightButton, leftButton;
    public Button rightSwap, leftSwap;
    public HUDScript HUD;
    public CameraShake shake;
    public DeathManager deathManager;
    public CameraManager cameraManager;
    public CharacterManager characterManager;
    public List<TimedBuff> CurrentBuffs = new List<TimedBuff>();

    public AudioClip healthRuneSound;

    public int runeHealth;
    public int potionHealth;

    public float missChance = 0;
    public Animator missAnimator;

    private bool unbreackble;
    private bool oneFromeHeroIsDead;

    // -1 or 1
    private float dodgeSide;

    private void Awake()
    {
        foreach (var sound in SoundManager.Sounds)
        {
            sound.Stop();
        }

        characterManager.SetCharacter();
        //PlayerPrefs.SetInt("RuneCurrentСount", 2);
        //PlayerPrefs.SetString("RuneCurrent0", Runes.Health);
        //PlayerPrefs.SetString("RuneCurrent1", Runes.BoneCrush);
        //PlayerPrefs.SetInt($"Rune{Runes.Health}",2);
        //PlayerPrefs.SetInt($"Rune{Runes.BoneCrush}", 1);
        oneFromeHeroIsDead = false;
        unbreackble = false;
        warior.GetComponent<Char>().cameraShake = shake;
        warior.GetComponent<Char>().cameraManager = cameraManager;
        warior.GetComponent<Char>().HudTransform = HUD.wariorHealthBar.transform.parent;
        warior = Instantiate(warior, transform);
        warior.name = "warior";
        ranger.GetComponent<Char>().cameraShake = shake;
        ranger.GetComponent<Char>().cameraManager = cameraManager;
        ranger.GetComponent<Char>().HudTransform = HUD.rangerHealthBar.transform.parent;
        ranger = Instantiate(ranger, transform);
        ranger.name = "ranger";

        characterManager.MoveCharacters();
        ClearAttackMod();
    }
    private void Start()
    {
        StartCoroutine(StartCoroutine());
    }

    private IEnumerator StartCoroutine()
    {
        yield return new WaitForEndOfFrame();
        points = 0;
        scoreText.text = points.ToString();
        Swap();
    }

    private void Update()
    {
        scoreText.text = points.ToString();
        if (Input.GetKeyDown(KeyCode.A))
            TurnLeft();
        else if (Input.GetKeyDown(KeyCode.D))
            TurnRight();
        else if (Input.GetKeyDown(KeyCode.R))
            Swap();


        foreach (TimedBuff buff in CurrentBuffs.ToArray())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                CurrentBuffs.Remove(buff);
            }
        }
    }

    public void TurnRight()
    {
        if (mageBl)
        {
            if (ranger.transform.localScale.x > 0)
                ranger.transform.localScale = new Vector3(-1f, 1, 1);
            ranger.GetComponent<Char>().Punch();
            ranger.GetComponent<Char>().TurnRight();
        }
        else
        {
            if (warior.transform.localScale.x > 0)
                warior.transform.localScale = new Vector3(-1f, 1, 1);
            warior.GetComponent<Char>().Punch();
            warior.GetComponent<Char>().TurnRight();
        }
    }

    public void TurnLeft()
    {
        if (mageBl)
        {
            if (ranger.transform.localScale.x < 0)
                ranger.transform.localScale = new Vector3(1f, 1f, 1f);
            ranger.GetComponent<Char>().Punch();
            ranger.GetComponent<Char>().TurnLeft();
        }
        else
        {
            if (warior.transform.localScale.x < 0)
                warior.transform.localScale = new Vector3(1f, 1, 1);
            warior.GetComponent<Char>().Punch();
            warior.GetComponent<Char>().TurnLeft();
        }
    }

    public void Swap()
    {
        wariorBl = !wariorBl;
        mageBl = !mageBl;
        ranger.GetComponent<Animator>().SetBool("chill", mageBl);
        warior.GetComponent<Animator>().SetBool("chill", wariorBl);
        if (mageBl)
        {
            Chill(ranger, warior, 1,HUD.mageButtons,HUD.wariorButtons);
        }
        else
        {
            Chill(warior, ranger, -1,HUD.wariorButtons,HUD.mageButtons);
        }
    }

    public void SwapIfNeeded(bool isWarior)
    {
        if (isWarior != wariorBl)
        {
            Swap();
        }
    }

    public void StopUnbreakeable()
    {
        unbreackble = false;
    }

    public void StopDamage()
    {
        warior.GetComponent<Animator>().SetBool("damage", false);
        ranger.GetComponent<Animator>().SetBool("damage", false); ;
    }

    public void EnableSkills()
    {
        if (!ranger.GetComponent<Char>().IsDead)
        {
            for (int i = 0; i < HUD.mageButtons.transform.childCount; i++)
            {
                HUD.mageButtons.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
            }
        }

        if (!warior.GetComponent<Char>().IsDead)
        {
            for (int i = 0; i < HUD.wariorButtons.transform.childCount; i++)
            {
                HUD.wariorButtons.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
            }
        }

        HUD.pauseButton.interactable = true;
    }

    public void DisableSkills()
    {
        for (int i = 0; i < HUD.mageButtons.transform.childCount; i++)
        {
            HUD.mageButtons.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
        }
        for (int i = 0; i < HUD.wariorButtons.transform.childCount; i++)
        {
            HUD.wariorButtons.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
        }
        HUD.pauseButton.interactable = false;
    }

    public void EnableSwaping()
    {
        if (!oneFromeHeroIsDead)
        {
            rightSwap.interactable = true;
            leftSwap.interactable = true;
        }
    }

    public void DisableSwaping()
    {
        rightSwap.interactable =false;
        leftSwap.interactable = false;
    }

    public void EnableAttackButtons()
    {
        rightButton.interactable = true;
        leftButton.interactable = true;
    }

    public void DisableAttackButtons()
    {
        rightButton.interactable = false;
        leftButton.interactable = false;
    }

    public void EnableAll()
    {
        EnableAttackButtons();
        EnableSkills();
        EnableSwaping();
    }

    public void DisableAll()
    {
        DisableAttackButtons();
        DisableSkills();
        DisableSwaping();
    }

    public void Chill(GameObject persoToForward,GameObject personToBack,int k,GameObject ButtonsActive,GameObject ButtonsDeActive)
    {
        SwapForward(persoToForward, ButtonsActive, k);
        SwapBack(personToBack, ButtonsDeActive, k);
    }

    public void DodgeAttack()
    {
        if (mageBl)
        {
            dodgeSide = Mathf.Sign(ranger.transform.localScale.x);
            SwapBack(ranger, HUD.mageButtons, -1);
            Invoke("MageOnForward", 1);
        }
        else
        {
            dodgeSide = Mathf.Sign(warior.transform.localScale.x);
            SwapBack(warior, HUD.wariorButtons, 1);
            Invoke("WariorOnForward", 1);
        }
        DisableAttackButtons();
        DisableSwaping();
        unbreackble = true;
    }

    public void HeroesDeath()
    {
        deathManager.EndOfGame();
    }

    public void SetUpAttackMod(IHaveAttackModifier attackModifier)
    {
        ranger.GetComponent<Char>().punchObject.GetComponent<PunchScript>().debuffs.Add(attackModifier.AttackModificator);
        warior.GetComponent<Char>().punchObject.GetComponent<PunchScript>().debuffs.Add(attackModifier.AttackModificator);
    }

    public void ClearAttackMod()
    {
        ranger.GetComponent<Char>().punchObject.GetComponent<PunchScript>().debuffs.Clear();
        warior.GetComponent<Char>().punchObject.GetComponent<PunchScript>().debuffs.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttack" && !unbreackble)
        {
            if (collision.GetComponent<Rigidbody2D>())
            {
                Destroy(collision.gameObject);
            }
            unbreackble = true;
            Invoke("StopUnbreakeable", 1);
            Invoke("StopDamage", 0.1f);
            if (mageBl)
            {
                TakingDamage(ranger);
            }
            else if (wariorBl)
            {
                TakingDamage(warior);
            }
            HUD.UpdateHeards();
        }
    }

    private void TakingDamage(GameObject character)
    {
        var miss = false;
        if (missChance > Random.Range(0, 100))
        {
            missAnimator.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), missAnimator.transform.position.y);
            missAnimator.SetTrigger("Miss");
            miss = true;

        }
        else if (potionHealth > 0)
        {
            potionHealth--;
        }
        else if (runeHealth > 0)
        {
            runeHealth--;
            SoundManager.PlaySoundUI(healthRuneSound);
        }
        else
        {
            character.GetComponent<Char>().TakingDamage();
        }

        if (!miss)
        {
            character.GetComponent<Animator>().SetBool("damage", true);
        }

        if (character.GetComponent<Char>().currentHeroHP == 0)
        {
            var characterButtons = wariorBl ? HUD.wariorButtons : HUD.mageButtons;
            if (oneFromeHeroIsDead)
            {
                character.GetComponent<Char>().Death();
                character.GetComponent<Animator>().SetBool("death", true);
                HeroesDeath();
                DisableAttackButtons();
            }
            else
            {
                character.GetComponent<Char>().Death();
                for (int i = 0; i < characterButtons.transform.childCount; i++)
                {
                    characterButtons.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
                    characterButtons.transform.GetChild(i).GetChild(0).GetComponent<Cooldown>().heroDead = true;
                }
                Swap();
                DisableSwaping();
                oneFromeHeroIsDead = true;
            }
        }
    }

    private void SwapForward(GameObject person1, GameObject ButtonsActive, int k)
    {
        //for (int i = 0; i < ButtonsActive.transform.childCount; i++)
        //{
        //    ButtonsActive.transform.GetChild(i).GetComponent<Button>().interactable = true;
        //}

        person1.transform.localPosition = new Vector3(0, 0, 0);
        person1.transform.localScale = new Vector3(1f * k, 1f, 1f);
        person1.GetComponent<SpriteRenderer>().color = Color.white;
        person1.GetComponent<SpriteRenderer>().sortingOrder = 50;
        person1.GetComponent<Animator>().SetBool("chill", !person1.GetComponent<Animator>().GetBool("chill"));
        person1.GetComponent<Char>().SwapForward();
    }

    private void SwapBack(GameObject person2, GameObject ButtonsDeActive,int k)
    {
        //for (int i = 0; i < ButtonsDeActive.transform.childCount; i++)
        //{
        //    ButtonsDeActive.transform.GetChild(i).GetComponent<Button>().interactable = false;
        //}
        person2.transform.localPosition = new Vector3(1.2f * k, 0.3f, 0);
        person2.transform.localScale = new Vector3(0.85f * -k, 0.85f, 0.85f);
        person2.GetComponent<SpriteRenderer>().color = swapColor;
        person2.GetComponent<SpriteRenderer>().sortingOrder = 0;
        person2.GetComponent<Char>().SwapBack();
        if (!person2.GetComponent<Char>().IsDead)
        {
            person2.GetComponent<Animator>().SetBool("chill", !person2.GetComponent<Animator>().GetBool("chill"));
        }
        else person2.GetComponent<Animator>().SetBool("death", true);
    }

    private void MageOnForward()
    {
        SwapForward(ranger, HUD.mageButtons, -1);
        EnableAttackButtons();
        EnableSwaping();
        ranger.transform.localScale = new Vector2(dodgeSide, 1);
        //GetComponent<BoxCollider2D>().enabled = true;
        unbreackble = false;
    }

    public void AddBuff(TimedBuff buff)
    {
        if (!CurrentBuffs.Any(x => x.ToString() == buff.ToString()))
        {
            CurrentBuffs.Add(buff);
            buff.Activate();
        }
        else
        {
            var item = CurrentBuffs.FirstOrDefault(x => x.ToString() == buff.ToString());
            item.Update();
        }
    }

    private void WariorOnForward()
    {
        SwapForward(warior, HUD.wariorButtons, 1);
        EnableAttackButtons();
        EnableSwaping();
        warior .transform.localScale = new Vector2(dodgeSide, 1);
        //GetComponent<BoxCollider2D>().enabled = true;
        unbreackble = false;
    }
}
