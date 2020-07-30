using Assets.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Frog : EnemyScript, IUseCamerManager
{
    public float jumpRate = 2;
    public Vector2 jumpVector = new Vector2(1, 2);
    public Vector2 deathVector = new Vector2(-1, 3);
    public float jumpForce = 5;
    public ParticleSystem particles;
    public CameraManager CameraManager { get; set; }
    public float cameraSize = 2.5f;
    public float cameraTime = 5;
    public float removeTime = 7f;

    public GuidManager guidManager;

    private bool isSecret = true;
    private bool isJumping = true;
    private SpriteRenderer _spriteRenderer;


    protected override void CastomStart()
    {
        base.CastomStart();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.black;
        StartCoroutine(Jumping());
        CameraManager.SetTarget(gameObject, cameraSize, cameraTime, Vector3.zero);
    }

    public override void EnemyDeath()
    {
        base.EnemyDeath();
        StopAllCoroutines();
        CameraManager.SetCameraBasePosition();
        isJumping = false;
        _rigidbody2D.AddForce(deathVector * jumpForce);
        GetComponent<BoxCollider2D>().enabled = false;
        if (isSecret)
        {
            guidManager.Continue();
            guidManager.SkipSentence();
            guidManager.SkipSentence();
            guidManager.ShowDialog();
        }
        else
        {
            guidManager.Continue();
        }
    }

    private IEnumerator Jumping()
    {
        while (isJumping)
        {
            _animator.SetTrigger("jump");
            _rigidbody2D.AddForce(jumpVector * jumpForce);
            yield return new WaitForSeconds(isSecret ? jumpRate : (jumpRate / 1.3f));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackZone")
        {
            particles.Stop();
            isJumping = false;
            _animator.SetTrigger("kva");
            StartCoroutine(AfterKva());
            _spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator AfterKva()
    {
        yield return new WaitForSeconds(jumpRate*2);
        CameraManager.SetCameraBasePosition();
        isJumping = true;
        isSecret = false;
        StartCoroutine(Jumping());
        guidManager.SkipSentence();
        guidManager.Continue();
        guidManager.ShowDialog();
        StartCoroutine(AfterRemove());
    }

    private IEnumerator AfterRemove()
    {
        yield return new WaitForSeconds(removeTime);
        if (!isDead)
        {
            Destroy(gameObject);
            guidManager.SkipSentence();
            guidManager.Continue();
        }
    }
}
