using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snich : EnemyScript, IUseIngredientManager
{
    public float traectoryUp = 0;
    public float yKoleb = 1;
    public float xKoleb = 1;
    public ParticleSystem particles1;
    public ParticleSystem particles2;
    public const float yStartPosition = -3f;
    private SpriteRenderer spriteRenderer;

    public IngredientManager IngredientManager { get; set; }

    protected override void CastomStart()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        yKoleb = Random.Range(0.3f, 4.00f);
        traectoryUp = yKoleb + yStartPosition;
        transform.localScale = new Vector3(0.3f * Mathf.Sign(transform.localScale.x), 0.3f);
    }

    protected override void CastomUpdate()
    {
        var y = traectoryUp + yKoleb * Mathf.Sin(xKoleb * transform.position.x);
        transform.position = new Vector2(transform.position.x, y);
    }

    public override void EnemyDeath()
    {
        base.EnemyDeath();
        IngredientManager.SpawnIngredient(this);
        spriteRenderer.enabled = false;
        particles1.Play();
        particles2.Play();
    }
}
