using Assets.Mappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GuidManager : MonoBehaviour
{
    public TextWriter textWriter;
    public Animator bardAnimator;
    public Animator dialogAnimator;

    public List<GuidStage> stages;

    private GuidStage currentStage;

    private bool clickSkip;
    private bool anySkipEvent;
    private Swipes swipeSkip;

    [NonSerialized]
    public UnityEvent skipEvent;

    private Vector3 bardStartPosition;
    private Vector3 bardStartScale;

    #region Singleton
    private static GuidManager _instance;

    private GuidManager()
    {
    }

    public static GuidManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public void Awake()
    {
        _instance = this;
        //GuidMapper.SetStageStatus(GuidStages.Initial, GuidStatus.NotActive);
        skipEvent = new UnityEvent();
        bardStartPosition = bardAnimator.GetComponent<RectTransform>().anchoredPosition;
        bardStartScale = bardAnimator.GetComponent<RectTransform>().localScale;
        foreach (var stage in stages)
        {
            stage.startStageAction.AddListener(ShowDialog);
            stage.endStageAction.AddListener(HideDialog);
        }
    }

    public void SkipWithDelay(float delay)
    {
        if (currentStage != null)
        {
            StartCoroutine(SkipWithDelayCoroutine(delay));
        }
    }

    private IEnumerator SkipWithDelayCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Continue();
    }

    public void ChekSwipeSkip(Swipes swipe)
    {
        if (swipe == swipeSkip)
        {
            swipeSkip = Swipes.None;
            Continue();
        }
    }

    public void SetSwipeSkip(Swipes swipe)
    {
        swipeSkip = swipe;
    }

    public void SetSkipOnClick()
    {
        clickSkip = true;
    }

    public void ActivateStage(GuidStages stageName)
    {
        if (currentStage == null)
        {
            gameObject.SetActive(true);
            var stage = stages.First(x => x.stageName == stageName);
            stage.Activate();
            currentStage = stage;
            Continue();
        }

    }

    public void AddSkipAction(UnityAction action)
    {
        skipEvent.AddListener(action);
        anySkipEvent = true;
        Debug.Log(skipEvent.GetPersistentEventCount());
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
        dialogAnimator.SetTrigger("show");
        bardAnimator.SetTrigger("movingForward");
    }

    public void HideDialog()
    {
        dialogAnimator.SetTrigger("close");
        bardAnimator.SetTrigger("movingBack");
        //StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (anySkipEvent || clickSkip))
        {
            if (anySkipEvent)
            {
                skipEvent.Invoke();
                skipEvent.RemoveAllListeners();
                anySkipEvent = false;
            }
            else
            {
                clickSkip = false;
            }
            Continue();
        }
    }

    public void Continue()
    {
        if (currentStage != null)
        {
            var sentence = currentStage.GetSentence();
            if (sentence != null)
            {
                WriteSentence(sentence);
            }
            else
            {
                currentStage.Finish();
                currentStage = null;
            }
        }
    }

    public void SkipSentence()
    {
        currentStage.SkipSentence();
    }

    private void WriteSentence(Sentence sentence)
    {
        textWriter.TypeText(sentence.sentenceText);

        bardAnimator.SetTrigger(sentence.bardEmotions.ConvertToString());
        bardAnimator.GetComponent<RectTransform>().localScale = bardStartScale * sentence.side;
        bardAnimator.GetComponent<RectTransform>().anchoredPosition = bardStartPosition * sentence.side;

        if (sentence.textStyle == TextStyle.Shake)
        {
            textWriter.StartShaking();
        }
        if (sentence.action.GetPersistentEventCount() == 0)
        {
            clickSkip = true;
        }
        else
        {
            sentence.action.Invoke();
        }
    }
}

