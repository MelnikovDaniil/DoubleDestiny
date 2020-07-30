using Assets;
using Assets.Mappers;
using Assets.Minigames.EventCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerationScript : MonoBehaviour
{
    public int totalEnemyCount;
    private Step CurrentStep { get; set; }
    public List<Step> steps;
    private IEnumerator coroutine;
    public Transform enemySpace;
    public ResourceFactory resourceFactory;
    public float generationDelay = 3f;

    System.Random _random = new System.Random();
    private GameObject enemyBuffer;
    private int enemyOrder = 1;
    void Start()
    {
        if (GuidMapper.GetStageStatus(GuidStages.Initial) == GuidStatus.NotActive)
        {
            GuidManager.Instance.ActivateStage(GuidStages.Initial);
        }
        else
        {
            //Destroy(SoundManager.Instance.gameObject);
            steps[0].Stages[0].Time = ConfigurationMapper.StepTime;
            steps[0].StartStep();
            coroutine = Generation();
            DefineCurrentStep();
            if (ConfigurationMapper.RemoveMinigames)
            {
                DeleteAllMinigames();
            }
            StartGame(CurrentStep);
            totalEnemyCount = TotalEnemyCount();
        }
    }

    public void StartGame(Step step)
    {
        step.StartStep();
        if (ConfigurationMapper.StartWithBoss)
        {
            CurrentStep.IsBossFight = true;
        }
        StartCoroutine(coroutine);
        //Написать алгоритм включения Этап
    }
    public IEnumerator Generation()
    {
        yield return new WaitForSeconds(generationDelay);
        while (true)
        {
            if (!CurrentStep.IsBossFight)
            {
                GoSpawnStep();
                yield return new WaitForSeconds(CurrentStep.CurrentStage.CurrentRateSpawn);
            }
            else if (enemySpace.childCount == 0)
            {
                SpawnBoss();
                StopCoroutine(coroutine);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }
        
    }
    public void DefineCurrentStep()
    {
        Step curStep = null;
        foreach (Step item in steps)
        {
            if (item.IsActive)
                curStep = item;
        }
        CurrentStep = curStep;
    }
    public void GoSpawnStep()
    {
        if (CurrentStep.IsActive == false)
        {
            NextStep();
        }
        else
        {

            enemyBuffer = CurrentStep.GoSpawnStage();

            if (enemyBuffer != null)
            {
                SpawnEnemy(enemyBuffer);
            }
        }
    }

    public GameObject SpawnEnemy(GameObject enemy)
    {
        int randomSide = _random.Next(2);
        if (randomSide == 0) randomSide = -1;

        GameObject newEnemy;
        if (enemy.TryGetComponent(out GameEvent gameEvent))
        {
            var newEvent = Instantiate(gameEvent, enemySpace);
            newEvent.transform.position *= new Vector2(randomSide, 1);
            newEvent.transform.localScale *= new Vector2(randomSide, 1);
            newEvent.StartEvent();
            newEnemy = newEvent.gameObject;
        }
        else
        {
            newEnemy = Instantiate(enemy, enemySpace);
            newEnemy.transform.localScale = new Vector3(0.75f * randomSide, 0.75f, 0.75f);
            newEnemy.transform.position = new Vector3(randomSide * Random.Range(13f, 23f), -3.73f);
            newEnemy.transform.localPosition = new Vector3(
                newEnemy.transform.localPosition.x,
                newEnemy.transform.localPosition.y,
                enemyOrder++);
        }
        resourceFactory.EstablishDependencyForEnemy(newEnemy);
        return newEnemy;
    }

    public void NextStep()
    {
        CurrentStep.IsActive = false;
        CurrentStep = steps[steps.IndexOf(CurrentStep) + 1];
        CurrentStep.StartStep();
    }

    private void SpawnBoss()
    {
        resourceFactory.EstablishDependencyForEnemy(Instantiate(CurrentStep.Boss, new Vector3(10.85f, -4.27f, 0), Quaternion.Euler(0, 0, -180)));
    }

    private int TotalEnemyCount()
    {
        var mobsCount = CurrentStep.Stages.Sum(x => x.MobsList.Sum(y => y.currentMob + y.waveMobs));
        return mobsCount;
    }

    private void DeleteAllMinigames()
    {
        var stages = CurrentStep.Stages
            .Select(x => 
                x.MobsList.Where(mob => 
                    mob.mob.TryGetComponent<GameEvent>(out var t)));
        foreach (var stage in stages)
        {
            foreach (var enemy in stage)
            {
                enemy.currentMob = 0;
            }
        }
    }
}
