using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilDragon : MonoBehaviour
{
    public float xShift = -0.5f;
    public float yShift = -2.5f;
    public float angle = 1;
    public float speed = 2;

    public float yKoleb = 1;
    public float xKoleb = 1;

    public float changeFormulaAfterX = 1;

    public SpriteRenderer dragonIcon;
    public TrailRenderer trail;

    public Gradient valorGadient;
    public Gradient evilGradient;

    private float yPosition;
    private float xPosition;
    private float side;

    private float disablingTime = 5f;
    private float currentSpeed;
    // Update is called once per frame

    private void OnEnable()
    {
        currentSpeed = speed;
        StartCoroutine(DisableDragon());
    }

    private IEnumerator DisableDragon()
    {
        yield return new WaitForSeconds(disablingTime);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    void Update()
    {
        currentSpeed += 5 * Time.deltaTime;
        side = Mathf.Sign(transform.localScale.x) * -1;
        xPosition = transform.position.x + currentSpeed * Time.deltaTime * side;
        //if (Mathf.Abs(transform.position.x) < changeFormulaAfterX + xShift)
        //{
            yPosition = angle / xPosition + xShift * side;
            transform.position = new Vector3(xPosition, Mathf.Abs(yPosition) + yShift);
        //}
        //else
        //{
        //    yPosition = yShift + yKoleb * Mathf.Sin(xPosition + xShift / Mathf.PI );
        //    transform.position = new Vector3(xPosition, yPosition);
        //}

    }

    public void TransformToValor(Sprite icon)
    {
        if (Random.Range(0, 2) == 0)
        {
            dragonIcon.sprite = icon;
        }
        trail.colorGradient = valorGadient;
    }

    public void TransformToEvil(Sprite icon)
    {
        dragonIcon.sprite = icon;
        trail.colorGradient = evilGradient;
    }

    public void SetRandomTraectory()
    {
        angle = Random.Range(0.9f, 1.1f);
        xShift = Random.Range(-0.3f, 0.7f);
    }
}
