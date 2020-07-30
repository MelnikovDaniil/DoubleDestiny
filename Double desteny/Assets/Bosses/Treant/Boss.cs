using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyScript,
    IUseCameraShake,
    IUseHealthBar,
    IUseQuickTimeEventManager,
    IUseRain,
    IUseResourceFactory
{
    public CameraShake CameraShake { get; set; }
    public BossHealthBar HealthBar { get; set; }
    public QuickTimeEventManager QuickTimeEventManager { get; set; }
    public ParticleSystem Rain { get; set; }
    public ResourceFactory ResourceFactory { get; set; }

    public ParticleSystem particlesInvisibility;
    public List<string> frontAnimations;
    public List<string> backAnimations;
    public int chanseOfWaiting;
    public bool onFront;
    public GameObject snag;
    public float invisibialityCoof;
    public ParticleSystem eye1, eye2;
    public ParticleSystem dirt;

    public AudioClip puff;

    public int snagCount;
    private string previosAnim;
    private int stadiaNumber;
    private bool isInvisile;
    private static int currentSnagCount;
    private Coroutine snagSpawnCoroutine;

    protected override void CastomStart()
    {
        base.CastomStart();
        stadiaNumber = 3;
        HealthBar.ShowHealthBar(this);
        isInvisile = false;
    }

    protected override void CastomUpdate()
    {
        if (stadiaNumber == 1 && isInvisile)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, invisibialityCoof);
        }
    }

    public override void EnemyDeath()
    {
        foreach (var buff in CurrentBuffs)
        {
            buff.End();
        }
        CharactersScript.points += 100;
        standingEnemy = true;
        isDead = true;
        GetComponent<Animator>().SetBool("death", true);
        currentSnagCount = 0;
        snagCount = 0;
        CancelInvoke();
        CancelInvoke("SnagNearTreant");
        StopCoroutine(snagSpawnCoroutine);
    }

    public void ShakeMoving()
    {
        StartCoroutine(CameraShake.Shake(0.2f, 0.015f));
    }
    public void ShackePunch()
    {
        StartCoroutine(CameraShake.Shake(1.5f, 0.15f));
        dirt.Play();
    }
    public void ParticleInvisibility()
    {
        particlesInvisibility.Play();
    }

    public void FrontAnimations()
    {
        var animationsAbsent = 1;
        if (stadiaNumber == 3)
        {
            animationsAbsent = 2;
        }
        onFront = true;
        if (stadiaNumber == 1 && !isInvisile)
        {
            GetComponent<Animator>().Play("Treant_MoovingBack", 0, 0);
        }
        else if (Random.Range(0, 100) < (previosAnim == "wait" ? chanseOfWaiting / 5 : chanseOfWaiting))
        {
            Wait();
        }
        else
        {
            if (frontAnimations.Contains(previosAnim) && stadiaNumber == 3)
            {
                previosAnim = frontAnimations[0];
            }
            else
            {
                previosAnim = frontAnimations[Random.Range(0, frontAnimations.Count - animationsAbsent)];
            }
            GetComponent<Animator>().Play(previosAnim, 0, 0);
        }
        if (stadiaNumber == 1)
        {
            //CancelInvoke("SnagRandomSide");
            //if (currentHP > 0)
            //{
            //    InvokeRepeating("SnagRandomSide", 0, 2.5f);
            //}
            //else
            //{
            //    CancelInvoke();
            //}
        }
    }

    public void BackAnimations()
    {
        var animationsAbsent = 1;
        if (stadiaNumber == 3)
        {
            animationsAbsent = 2;
        }
        onFront = false;

        if (stadiaNumber == 1 && !isInvisile)
        {
            GetComponent<Animator>().Play("Treant_Invisible", 0, 0);
        }
        else if (Random.Range(0, 100) < (previosAnim == "wait" ? chanseOfWaiting / 5 : chanseOfWaiting))
        {
            Wait();
        }
        else
        {
            previosAnim = backAnimations[Random.Range(0, backAnimations.Count - animationsAbsent)];
            GetComponent<Animator>().Play(previosAnim, 0, 0);
        }
    }

    public void PlayRandomAnim()
    {
        if(onFront)
        {
            FrontAnimations();
        }
        else
        {
            BackAnimations();
        }
    }

    public void Invisibilyty()
    {
        GetComponent<SpriteRenderer>().color = new Color(225, 225, 225, 0);

        if (stadiaNumber <= 2)
        {
            CallSnags();
        }
        Invoke("RandomTeleport", 2);
    }

    public void CallSnags()
    {
        currentSnagCount = snagCount;
        InvokeRepeating("SnagNearTreant", 0, 0.3f);
        GetComponent<AudioSource>().Play(0);
    }

    public void Wait()
    {
        previosAnim = "wait";
        GetComponent<Animator>().Play("TreantIdle", 0, 0);
    }

    public void Wait(bool onFront)
    {
        Wait();
        this.onFront = onFront;
    }

    public void MakeVisible()
    {
        GetComponent<SpriteRenderer>().color = new Color(225, 225, 225);
        GetComponent<Animator>().Play("Treant_Visible", 0, 0);
    }

    public void Shoot()
    {
        var spawnPosition = new Vector3(transform.position.x, transform.position.y+2);
        float l = Vector3.Distance(spawnPosition, new Vector3(1 * Mathf.Sign(transform.position.x), -2.78f, 0));
        GameObject obj = Instantiate(snag, new Vector3(spawnPosition.x - 0.3f * Mathf.Sign(spawnPosition.x), spawnPosition.y, 0), Quaternion.identity);
        float f = Mathf.Sqrt(l * 9.8f / 1);
        obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        obj.tag = "EnemyAttack";
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * Mathf.Sign(transform.localScale.x), obj.transform.localScale.y);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(65f * f / -Mathf.Sign(transform.position.x), 250 * f));
        Destroy(obj, 2.5f);
    }

    protected override void CastomTackingDamage()
    {
        base.CastomTackingDamage();
        HealthBar.UpdateInfo();
        if (stadiaNumber > 2 && currentHP / HP < (float)2 / 3)
        {
            stadiaNumber = 2;
            chanseOfWaiting = 10;
            Rain.Play();
        }
        if (stadiaNumber > 1 && currentHP / HP < (float)1 / 3)
        {
            stadiaNumber = 1;
            chanseOfWaiting = 0;
            HealthBar.Lightning();
            snagSpawnCoroutine = StartCoroutine(SnagSpawnCoroutine());
            var emissionModule = Rain.emission;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(100, 200);
            eye1.Play();
            eye2.Play();
        }
    }

    private IEnumerator SnagSpawnCoroutine()
    {
        while (true)
        {
            SnagRandomSide();
            yield return new WaitForSeconds(2.5f);
        }
    }

    private void SnagNearTreant()
    {
        var snagEnemy = Instantiate(snag, new Vector3(transform.position.x + Random.Range(-3.5f, 3.5f), transform.position.y + Random.Range(-3.0f, 3.0f), 0), Quaternion.identity);
        snagEnemy.transform.localScale = new Vector3(snagEnemy.transform.localScale.x * Mathf.Sign(transform.localScale.x), snagEnemy.transform.localScale.y);
        SpawnOneSnag(snagEnemy);
        currentSnagCount--;
        if (currentSnagCount <= 0)
        {
            CancelInvoke("SnagNearTreant");
        }
    }

    private void SnagRandomSide()
    {
        var k = 1;
        var r = Random.Range(-1, 1);
        if (r == -1)
        {
            k = r;
        }

        var snagEnemy = Instantiate(snag, new Vector3((k*7.24f) + Random.Range(-3.5f, 3.5f), transform.position.y + Random.Range(-3.0f, 3.0f), 0), Quaternion.identity);
        snagEnemy.transform.localScale = new Vector3(snagEnemy.transform.localScale.x * k/*Mathf.Sign(transform.localScale.x)*/, snagEnemy.transform.localScale.y);
        SpawnOneSnag(snagEnemy);
    }

    private void SpawnOneSnag(GameObject snagEnemy)
    {
        SoundManager.PlaySound(puff);
        ResourceFactory.EstablishDependencyForEnemy(snagEnemy);
        var colorNum = Random.Range(0.65f, 1.00f);
        var color = Color.black;
        color.r += colorNum;
        color.b += colorNum;
        color.g += colorNum;
        snagEnemy.GetComponent<SpriteRenderer>().material.color = color;
        snagEnemy.GetComponent<SpriteRenderer>().sortingOrder += (int)Mathf.Round(colorNum - 0.30f); 
    }

    private void RandomTeleport()
    {
        // Добавить шанс того что босс не переместиться и шанс того что босс телепортируктся в упор к персам
        var onFrontX = 4.209415f * Mathf.Sign(transform.position.x);
        float coordX = 0;
        if (Random.Range(0,2) == 1)
        {
            onFront = true;
            coordX = onFrontX;
        }
        else
        {
            coordX = transform.position.x;
        }
        if (Random.Range(0, 2) == 1)
        {
            transform.position = new Vector3(coordX, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(-coordX, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.position.z);
        }
        if (stadiaNumber >= 2)
        {
            ParticleInvisibility();
            Invoke("MakeVisible", 1.5f);
        }
        else
        {
            invisibialityCoof = 0;
            isInvisile = true;
            PlayRandomAnim();
        }
    }

    public void StartQuickTime()
    {
        QuickTimeEventManager.GenerateEvent();
    }
}
