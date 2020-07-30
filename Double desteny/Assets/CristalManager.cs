using Assets.Mappers;
using Assets.Minigames.EventCore;
using Assets.Runes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CristalManager : MonoBehaviour
{
    public SpriteRenderer cristalPrefab;
    public float hideTime = 1;

    public float xForce = 25;
    public float yForce = 50;

    public float crystalAngle = 30f;

    public List<Sprite> cristals;

    [NonSerialized]
    public List<CristalInfo> savedCrystals;
    private const int CristalColorCount = 3;

    private void Start()
    {
        savedCrystals = new List<CristalInfo>();
    }

    public void SpawnCrystal(GameEvent enemy, CristallLevelEnum craftingLevel, int cristalCount)
    {
        var status = GuidMapper.GetStageStatus(GuidStages.Runes);
        if (status == GuidStatus.NotActive)
        {
            GuidMapper.SetStageStatus(GuidStages.Runes, GuidStatus.WhaitingForActivation);
        }

        for (int i = 0; i < cristalCount; i++)
        {
            var cristallColorNumber = Random.Range(0, CristalColorCount);
            var randomCristall = cristals[cristallColorNumber + ((int)craftingLevel * CristalColorCount)];

            var cristal = Instantiate(cristalPrefab, enemy.transform.position, Quaternion.identity, transform);
            cristal.sprite = randomCristall;
            cristal.transform.eulerAngles = Vector3.forward * Random.Range(-crystalAngle, crystalAngle);
            RuneMapper.PlusCristall((CristallColorEnum)cristallColorNumber, craftingLevel, 1);
            savedCrystals.Add(new CristalInfo
            {
                CristallColor = (CristallColorEnum)cristallColorNumber,
                CristallLevel = craftingLevel,
            });

            StartCoroutine(ExecuteAfterTime(cristal.gameObject));
            var forceVector = new Vector2(Random.Range(-10.0f, 10.0f) * xForce, Random.Range(5f, 10.0f) * yForce);
            cristal.GetComponent<Rigidbody2D>().AddRelativeForce(forceVector);
        }
    }

    private IEnumerator ExecuteAfterTime(GameObject cristal)
    {
        yield return new WaitForSeconds(hideTime);
        Destroy(cristal, 1);
        // Code to execute after the delay
    }
}
