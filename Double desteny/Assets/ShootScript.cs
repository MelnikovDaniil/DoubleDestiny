using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootScript : PunchScript
{
    public float speed;
    public GameObject explotion;
    protected bool isTouchDestroy;
    public AudioClip shootSound;
    public AudioClip shellEnemySound;

    private SMSound _currentShootSound;
    // Update is called once per frame
    private void Start()
    {
        if (shootSound)
        {
            _currentShootSound = SoundManager.PlaySound(shootSound).SetVolume(0.3f);
        }
        isTouchDestroy = true;
        Destroy(gameObject,2);
    }

    void Update()
    {
        transform.Translate(new Vector3(-1 * speed * Time.deltaTime * Mathf.Round(transform.localScale.x), 0, 0));
    }
    public override void TriggerEvent(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>() && collision.gameObject.GetComponent<EnemyScript>().isDead == false)
        {
            ShootAbilities(collision);
            Destroy(Instantiate(explotion, transform.position, Quaternion.identity), 1);
            base.TriggerEvent(collision);
            if (isTouchDestroy)
            {
                if (shootSound)
                {
                    SoundManager.PlaySound(shellEnemySound);
                    //_currentShootSound.Stop();
                }
                Destroy(gameObject, 0.1f);
            }
        }
    }
    public virtual void ShootAbilities(Collider2D collision)
    {

    }
}
