using Assets.BuffSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public float HP;
    public float currentHP;
    protected bool standingEnemy;
    public bool standartAttack;
    public float rateEnemyDamage;
    public GameObject popupDamage;
    public ParticleSystem damageParticle;
    public List<TimedBuff> CurrentBuffs = new List<TimedBuff>();

    public float soundVolume = 1;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip punchSound;
    public CoinManager coinManager;
    public int coinReward;
    
    public bool isDead;

    protected Animator _animator;
    protected Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        standingEnemy = false;
        Physics2D.IgnoreLayerCollision(8, 8,true);
        currentHP = HP;
        CastomStart();

    }

    protected virtual void CastomStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!standingEnemy)
        transform.Translate(new Vector3(-1*speed*Time.deltaTime * Mathf.Sign(transform.localScale.x), 0, 0));

        foreach (TimedBuff buff in CurrentBuffs.ToArray())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                CurrentBuffs.Remove(buff);
            }
        }

        CastomUpdate();
    }


    protected virtual void CastomUpdate()
    {

    }

    public void TakingDamage(float damage, float repulsion)
    {
        TakingDamage(damage, repulsion, false);
    }

    public void TakingDamage(float damage,float repulsion,bool isShoot)
    {
        if (!isDead)
        {
            ParticleSystem p = damageParticle;
            GameObject popupObject = Instantiate(popupDamage,new Vector3(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.2f,0.2f), 1),Quaternion.identity);
            popupObject.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            Destroy(popupObject, 0.5f);
            if (damageSound)
            {
                SoundManager.PlaySound(damageSound).SetVolume(soundVolume);
            }
            SetupParticle(p, isShoot);
            p.Play();
            if (currentHP > damage)
            {
                
                currentHP -= damage;
                _rigidbody2D.AddForce(new Vector3(100 * Mathf.Sign(transform.position.x) * repulsion, 50 * repulsion, 0));
            }
            else
            {
                EnemyDeath();
                damageParticle.Play();  
            }
            CastomTackingDamage();
            _animator.Play("EnemyDamage",1); // .SetBool("damage", true);
            //Invoke("stopDamage", 0.1f);
        }
    }
    

    public virtual void EnemyDeath()
    {
        if (deathSound)
        {
            SoundManager.PlaySound(deathSound).SetVolume(soundVolume);
        }
        foreach (var buff in CurrentBuffs)
        {
            buff.End();
        }
        CharactersScript.points++;
        standingEnemy = true;
        isDead = true;
        _animator.SetBool("death", true);
        if (gameObject != null)
        {
            Destroy(gameObject, 2f);
        }
        coinManager.SpawnCoins(this);
        CancelInvoke();
    }

    public void PlayPunchSound()
    {
        SoundManager.PlaySound(punchSound).SetVolume(soundVolume);
    }

    public void DontAttack()
    {
        _animator.SetBool("attack", false);
        standingEnemy = false;
        CancelInvoke();
    }
    

    public void stopDamage()
    {
        _animator.SetBool("damage", false);
    }

    public void Attack()
    {
        _animator.Play("Attack",0);
    }

    public void AddBuff(TimedBuff buff)
    {
        if (!isDead)
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
    }

    protected virtual void CastomTackingDamage()
    {
        
    }

    protected virtual void SetupParticle(ParticleSystem particleSystem, bool isShoot)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackZone" && standartAttack)
        {
            _animator.SetBool("attack", true);
            InvokeRepeating("Attack", 0, rateEnemyDamage);
            standingEnemy = true;
            //GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x + 1, GetComponent<BoxCollider2D>().size.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AttackZone" && !collision.IsTouching(GetComponent<Collider2D>()) && !isDead)
        {
            Invoke("DontAttack", 0.5f);
            //GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x - 1, GetComponent<BoxCollider2D>().size.y);
        }
    }
}
