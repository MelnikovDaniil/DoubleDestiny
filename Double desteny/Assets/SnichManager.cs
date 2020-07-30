using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnichManager : MonoBehaviour
{
    public int gameToSnich = 3;
    public float maxSecondsToSnich = 300;
    public float minSecondsToSnich = 0;
    public EnemyGenerationScript enemyGenerator;
    public Snich snichPrefab;

    private void Start()
    {
        PotionMapper.AddGameToSnish();
        if (PotionMapper.GetGameToSnish() >= gameToSnich)
        {
            StartCoroutine(SpawnSnich());
        }
    }

    private IEnumerator SpawnSnich()
    {
        var secondsToSnich = Random.Range(minSecondsToSnich, maxSecondsToSnich);
        yield return new WaitForSeconds(secondsToSnich);
        enemyGenerator.SpawnEnemy(snichPrefab.gameObject);
        PotionMapper.RefreshSnich();
    }
}
