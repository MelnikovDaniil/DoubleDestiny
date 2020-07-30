using Assets;
using Assets.Characters;
using Assets.Characters.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpecialType
{
    Procent,
    Integer,
    Time,
}
public abstract class Char : MonoBehaviour
{
    public virtual string Name { get; set; }

    public bool preview;
    public GameObject punchObject;
    public float damage;
    public float[] damageMas;
    public float[] special;
    public CharacterType characterType;
    public SpecialType specialType;
    public WayEnum way;
    public int heroHP;
    public int currentHeroHP;
    public bool Shaking;
    public CameraShake cameraShake;
    public bool IsDead;
    public Sprite icon;
    public Sprite skillIcon;
    public Sprite evilSkillIcon;
    public Sprite valorSkillIcon;
    public Sprite evilSprite;
    public Sprite valorSprite;
    public Transform HudTransform;
    public CameraManager cameraManager;
    public float repulsion;
    public Skill firstSkill,secondSkill;
    public GameObject[] valorObjects;
    public GameObject[] evilObjects;
    public bool nonPlayed;
    public SpecialBar specialBar;

    public AudioClip takingDamageSound;
    public AudioClip punchSound;
    public List<AudioClip> otherSounds;
    protected Animator _animator;
    protected int specialLevel;

    [TextArea]
    public string description;

    [TextArea]
    public string valorSkillDescription;

    [TextArea]
    public string evilSkillDescription;

    // Start is called before the first frame update
    void Awake()
    {
        AwakeMethod();
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartMethod();
    }
    public virtual void TakingDamage()
    {
        if (takingDamageSound)
        {
            SoundManager.PlaySound(takingDamageSound).SetVolume(0.5f);
        }
        currentHeroHP--;
    }
    public virtual void Punch()
    {
        if (gameObject.activeSelf)
        {
            if (punchSound)
            {
                SoundManager.PlaySound(punchSound).SetVolume(0.3f);
            }
            _animator.Play("Punch");
        }
        if (Shaking)
            StartCoroutine(cameraShake.Shake(0.20f, 0.06f));
    }

    public virtual void Shot()
    {
        punchObject.transform.localScale = new Vector3(0.65f * Mathf.Sign(transform.localScale.x), 0.65f, 0.65f);
        Instantiate(punchObject, new Vector3(transform.parent.position.x - 1.5f * Mathf.Sign(transform.localScale.x), transform.parent.position.y + 0.5f), Quaternion.identity);
    }

    public virtual void AwakeMethod()
    {
        heroHP = PlayerPrefs.GetInt(Name+ "ValueHP", 1);
        if (PlayerPrefs.GetInt(Name + "ValueFirstSkill") == 0)
            firstSkill.purchased = false;
        if (PlayerPrefs.GetInt(Name + "ValueSecondSkill") == 0)
            secondSkill.purchased = false;
        currentHeroHP = heroHP;

        specialLevel = PlayerPrefs.GetInt(Name + "ValueSpecial");
    }
    public virtual void StartMethod()
    {
        if (!nonPlayed)
        {
            specialBar = Instantiate(specialBar, HudTransform);

            if (PlayerPrefs.GetInt(Name + "ValueFirstSkill") > 0)
            {
                firstSkill.purchased = true;
            }
            if (PlayerPrefs.GetInt(Name + "ValueSecondSkill") > 0)
            {
                secondSkill.purchased = true;
                if (PlayerPrefs.GetInt(Name + "ValueIsValor") == 1)
                {
                    way = WayEnum.Valor;
                    TransformToValor();
                }
                else
                {
                    way = WayEnum.Evil;
                    TransformToEvil();
                }
            }

            specialBar.CreateSkillBar();
        }

        damage = damageMas[PlayerPrefs.GetInt(Name + "ValueDamage")];
        punchObject.GetComponent<PunchScript>().damage = damage;
        punchObject.GetComponent<PunchScript>().repulsion = repulsion;
    }

    public virtual void Death()
    {
        IsDead = true;
        specialBar.gameObject.SetActive(false);
    }

    public virtual void SwapForward()
    {

    }
    public virtual void SwapBack()
    {

    }
    public virtual void TurnRight()
    {

    }
    public virtual void TurnLeft()
    {

    }
    public virtual void TransformToValor()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = valorSprite;

        foreach (var valorItem in valorObjects)
        {
            valorItem.SetActive(true);
        }

        specialBar.skillIcon = valorSkillIcon;
    }
    public virtual void TransformToEvil()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = evilSprite;

        foreach (var evilItem in evilObjects)
        {
            evilItem.SetActive(true);
        }

        specialBar.skillIcon = evilSkillIcon;
    }

    public SMSound PlayFromOtherSounds(string soundName)
    {
        return SoundManager.PlaySound(otherSounds.First(x => x.name == soundName));
    }
}
