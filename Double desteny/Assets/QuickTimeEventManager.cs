using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEventManager : MonoBehaviour
{
    public GameObject quickButton;
    public CharactersScript characters;
    public Transform quickTimeEventPanel;

    private GameObject currentQuickButton;

    //private void Start()
    //{
    //    InvokeRepeating("GenerateEvent", 0, 3f);
    //}

    public void GenerateEvent()
    {
        Time.timeScale = 0.15f;
        SoundManager.ChangePitchByTime();
        var button = Instantiate(quickButton,new Vector3(Random.Range(180.00f, 1080.00f), Random.Range(180.00f, 540.00f),0), Quaternion.identity,transform);
        button.transform.SetParent(quickTimeEventPanel);
        button.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(characters.DodgeAttack);
        button.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(SuccessEvent);
        currentQuickButton = button;
        Invoke("FinishEvent", 0.25f);
    }



    public void FinishEvent()
    {
        CancelInvoke("FinishEvent");
        Destroy(currentQuickButton, 1f);
        Time.timeScale = 1f;
        SoundManager.ChangePitchByTime();
    }

    public void SuccessEvent()
    {
        CancelInvoke("FinishEvent");
        Destroy(currentQuickButton);
        Time.timeScale = 1f;
        SoundManager.ChangePitchByTime();
    }
}
