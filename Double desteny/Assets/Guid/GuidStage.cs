using Assets.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

[Serializable]
public class GuidStage
{
    public GuidStages stageName;

    public UnityEvent startStageAction;

    public UnityEvent endStageAction;

    public List<Sentence> _sentences;

    private Queue<Sentence> sentencesCopy;

    public void Activate()
    {
        sentencesCopy = new Queue<Sentence>(_sentences);
        startStageAction.Invoke();
    }

    public void Finish()
    {
        GuidMapper.SetStageStatus(stageName, GuidStatus.Done);
        endStageAction.Invoke();
    }

    public void SkipSentence()
    {
        sentencesCopy.Dequeue();
    }

    public Sentence GetSentence()
    {
        if (sentencesCopy.Any())
        {
            return sentencesCopy.Dequeue();
        }
        return null;
    }
}
