using Assets.Buffs;
using Assets.Buffs.Frosty;
using Assets.BuffSystem;
using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public float damage;
    public bool IsDestroyed;
    public float destroyTime;
    public float repulsion;
    public bool isShot;
    public List<ScriptableBuff> debuffs;

    public bool isDisabling;
    public float disablingSeconds;
    private float currrneSeconds;

    // Start is called before the first frame update
    void Start()
    {
        if(IsDestroyed)
        Destroy(gameObject, destroyTime);
    }

    private void OnEnable()
    {
        currrneSeconds = disablingSeconds;
    }
    private void Update()
    {
        if (isDisabling)
        {
            currrneSeconds -= Time.deltaTime;
            if (currrneSeconds < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEvent(collision);
    }
    public virtual void TriggerEvent(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 8)
        {
            if (collision.gameObject.tag == "EnemyAttack" && collision.gameObject.GetComponent<ParticleSystem>())
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.GetComponent<EnemyScript>())
            {
                if (!collision.GetComponent<Boss>())
                {
                    foreach (var debuff in debuffs)
                    {
                        var randomChanse = Random.Range(0, 100);
                        if (debuff is IChanseOfTrigger && randomChanse < ((IChanseOfTrigger)debuff).ChanseOfTrigger)
                        {
                            collision.gameObject.GetComponent<EnemyScript>().AddBuff(debuff.InitializeBuff(collision.gameObject));
                        }
                    }
                }

                collision.gameObject.GetComponent<EnemyScript>().TakingDamage(damage, repulsion, isShot);
            }
        }
    }
}