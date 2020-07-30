using Assets.Mappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidEvents : MonoBehaviour
{
    public Button leftPanel;
    public Button rightPanel;
    public Button leftSwapButton;
    public Button rightSwapButton;

    public GameObject rangerHud;
    public GameObject wariorHud;

    public List<EnemyScript> enemies;
    public EnemyGenerationScript generator;
    public ResourceFactory resourceFactory;
    public Frog frog;

    public Canvas canvas;

    public GameObject bgPanel;

    private GuidManager guidManager;

    public List<GameObject> objectsToDisable;

    public WitcherMinigame witchMinigame;
    public MinotaurMinigame minotaurMinigame;

    public Animator swipeAnimator;

    private Queue<EnemyScript> enemyQueue;


    private void Start()
    {
        guidManager = GetComponent<GuidManager>();

        if (GuidMapper.GetStageStatus(GuidStages.SkillSecondStage) == GuidStatus.WhaitingForActivation)
        {
            guidManager.ActivateStage(GuidStages.SkillSecondStage);
        }
        else if (GuidMapper.GetStageStatus(GuidStages.Initial) != GuidStatus.NotActive)
        {
            gameObject.SetActive(false);
        }
    }

    #region StageEvent

    public void BeginingStageAction()
    {
        generator.enabled = false;
        enemyQueue = new Queue<EnemyScript>();
        EnemyScript enemyBuffer;
        foreach (var enemy in enemies)
        {
            var spawnedEnemy = generator.SpawnEnemy(enemy.gameObject);
            spawnedEnemy.SetActive(false);
            if (spawnedEnemy.TryGetComponent(out enemyBuffer))
            {
                enemyQueue.Enqueue(enemyBuffer);
            }
        }

        foreach (var obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

    }

    public void StartDeathStage()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 100;
    }

    public void EndDeathStage()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -6;
    }

    #endregion

    #region SentenceEvents

    #region RightPanel
    public void ShowRightPanelWithDelay(float delay)
    {
        StartCoroutine(ShowRightPanel(delay));
    }

    private IEnumerator ShowRightPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        //bgPanel.SetActive(true);
        rightPanel.gameObject.SetActive(true);
        rightPanel.onClick.AddListener(HideRightPanel);
        rightPanel.animator.enabled = true;
        rightPanel.animator.SetTrigger("panel");
    }

    private void HideRightPanel()
    {
        //bgPanel.SetActive(false);
        rightPanel.gameObject.SetActive(false);
        rightPanel.onClick.RemoveListener(HideRightPanel);
        rightPanel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        rightPanel.animator.enabled = false;
        guidManager.Continue();
    }

    #endregion

    #region LeftPanel
    public void ShowLeftPanelWithDelay(float delay)
    {
        StartCoroutine(ShowLeftPanel(delay));
    }
    private IEnumerator ShowLeftPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        //bgPanel.SetActive(true);
        leftPanel.gameObject.SetActive(true);
        leftPanel.onClick.AddListener(HideLeftPanel);
        
        leftPanel.animator.enabled = true;
        leftPanel.animator.SetTrigger("panel");
    }

    private void HideLeftPanel()
    {
        //bgPanel.SetActive(false);
        leftPanel.gameObject.SetActive(false);
        leftPanel.onClick.RemoveListener(HideLeftPanel);
        leftPanel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        leftPanel.animator.enabled = false;
        guidManager.Continue();
    }

    #endregion

    #region SwapButtons
    public void ShowSwapButtonsWithDelay(float delay)
    {
        StartCoroutine(ShowSwapButtons(delay));
    }
    private IEnumerator ShowSwapButtons(float delay)
    {
        yield return new WaitForSeconds(delay);
        bgPanel.SetActive(true);
        leftSwapButton.transform.parent.gameObject.SetActive(true);
        rightSwapButton.transform.parent.gameObject.SetActive(true);
        leftSwapButton.onClick.AddListener(HideSwapButtons);
        rightSwapButton.onClick.AddListener(HideSwapButtons);
    }

    private void HideSwapButtons()
    {
        bgPanel.SetActive(false);
        leftSwapButton.transform.parent.gameObject.SetActive(false);
        rightSwapButton.transform.parent.gameObject.SetActive(false);
        leftSwapButton.onClick.RemoveListener(HideSwapButtons);
        rightSwapButton.onClick.RemoveListener(HideSwapButtons);
        guidManager.Continue();
    }

    #endregion

    #region TwoPanels

    private int clickCount;

    public void ShowTwoPanelsWithDelay(float delay)
    {
        StartCoroutine(ShowTwoPanels(delay));
    }

    private IEnumerator ShowTwoPanels(float delay)
    {
        yield return new WaitForSeconds(delay);
        rangerHud.SetActive(true);
        rightPanel.gameObject.SetActive(true);
        leftPanel.gameObject.SetActive(true);
        rightPanel.onClick.AddListener(HideTwoPanels);
        leftPanel.onClick.AddListener(HideTwoPanels);
    }

    private void HideTwoPanels()
    {
        clickCount++;
        if (clickCount > 4)
        {
            rightPanel.gameObject.SetActive(false);
            leftPanel.gameObject.SetActive(false);
            rightPanel.onClick.RemoveListener(HideTwoPanels);
            leftPanel.onClick.RemoveListener(HideTwoPanels);
            guidManager.Continue();
        }
    }

    #endregion

    #region TwoPanels

    public void ShowManaWithDelay(float delay)
    {
        StartCoroutine(ShowMana(delay));
    }

    private IEnumerator ShowMana(float delay)
    {
        yield return new WaitForSeconds(delay);
        bgPanel.SetActive(true);
        guidManager.AddSkipAction(HideMana);
    }

    public void HideMana()
    {
        bgPanel.SetActive(false);
    }

    #endregion

    #region Frog

    public void SpawnFrogWithDelay(float delay)
    {
        StartCoroutine(SpawnFrog(delay));
    }

    private IEnumerator SpawnFrog(float delay)
    {
        yield return new WaitForSeconds(delay);
        leftPanel.gameObject.SetActive(true);
        rightPanel.gameObject.SetActive(true);
        var frogObj = Instantiate(frog, GameObject.Find("EnemySpace").transform);
        frogObj.guidManager = guidManager;
        resourceFactory.EstablishDependencyForEnemy(frogObj.gameObject);
        guidManager.HideDialog();
    }

    #endregion

    #region StartSpawnMonsters
    public void StartSpawnMonstersWithDelay(float delay)
    {
        StartCoroutine(StartSpawnMonsters(delay));
    }

    private IEnumerator StartSpawnMonsters(float delay)
    {
        var spawnRate = 0.5f;
        yield return new WaitForSeconds(delay);
        guidManager.Continue();
        foreach (var item in objectsToDisable)
        {
            item.SetActive(true);
        }
        while(enemyQueue.Count > 0)
        {
            var enemy = enemyQueue.Dequeue();
            enemy.gameObject.SetActive(true);
            enemy.HP += 20;
            enemy.speed += 2;
            enemy.coinReward = 0;
            if (enemyQueue.Count == 0)
            {
                enemy.HP += 1000;
                enemy.speed += 1;
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    #endregion

    #region Witch

    public void StopTimeWithDelay(float delay)
    {
        StartCoroutine(StopTime(delay));
    }

    public IEnumerator StopTime(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0;
        guidManager.SetSkipOnClick();
    }

    public void ReduceTimeAnd()
    {
        guidManager.AddSkipAction(SkipAndHideBard);
    }

    private void SkipAndHideBard()
    {
        Time.timeScale = 1;
        guidManager.HideDialog();
    }

    public void MinigameInteractionWithDelay(float delay)
    {
        StartCoroutine(MinigameInteraction(delay));
    }

    private IEnumerator MinigameInteraction(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        guidManager.ShowDialog();
        witchMinigame.StopAllPotions();
        guidManager.SetSkipOnClick();
    }

    public void SetRightSwipeWithDelay(float delay)
    {
        StartCoroutine(SetRightSwipe(delay));
    }

    private IEnumerator SetRightSwipe(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        guidManager.SetSwipeSkip(Swipes.Right);
        swipeAnimator.gameObject.SetActive(true);
        swipeAnimator.SetTrigger("swipeRight");
    }

    public void SetLeftSwipeWithDelay(float delay)
    {
        StartCoroutine(SetLeftSwipe(delay));
    }

    private IEnumerator SetLeftSwipe(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        guidManager.SetSwipeSkip(Swipes.Left);
        swipeAnimator.SetTrigger("swipeLeft");
    }

    #endregion

    #region Minotaur

    public void MinotaurMinigameInteractionWithDelay(float delay)
    {
        StartCoroutine(MinotaurMinigameInteraction(delay));
    }

    private IEnumerator MinotaurMinigameInteraction(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        guidManager.ShowDialog();
        minotaurMinigame.enemy.GetComponent<Animator>().enabled = false;
        minotaurMinigame.StopMinigame();
        guidManager.SetSkipOnClick();
    }

    public void ShowStickWithDelay(float delay)
    {
        StartCoroutine(ShowStick(delay));
    }

    private IEnumerator ShowStick(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GuidMapper.SetStageStatus(GuidStages.Minotaur, GuidStatus.WhaitingForActivation);   
        minotaurMinigame.character.StartMoving();
        minotaurMinigame.stick.gameObject.SetActive(true);
        var canvas = minotaurMinigame.stick.gameObject.AddComponent<Canvas>();
        bgPanel.SetActive(true);
        canvas.overrideSorting = true;
        canvas.sortingOrder = 110;
        guidManager.HideDialog();
    }

    public void AfterStick()
    {
        guidManager.ShowDialog();
        minotaurMinigame.character.StopMoving();
        var canvas = minotaurMinigame.stick.gameObject.GetComponent<Canvas>();
        bgPanel.SetActive(false);
        canvas.overrideSorting = false;
        Destroy(canvas);
    }

    #endregion

    #endregion

    #region Common
    
    public void Empty() { }
    
    #endregion
}
