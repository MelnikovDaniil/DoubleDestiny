using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public float timeScale;
    public MinigameController minigameController;
    public AudioClip panelEnableSound;
    public AudioClip panelDisableSound;

    private bool isPaused;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void MainMenu()
    {
        Load("MainMenu");
        SoundManager.PlaySoundUI("button");
    }

    public void ReloadLevel()
    {
        Load("game");
        SoundManager.PlaySoundUI("button");
    }

    public void LoadTavern()
    {
        Load("MainMenu");
        SoundManager.PlaySoundUI("button");
    }

    private void Load(string sceenName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceenName);
    }

    public void PauseOrResume()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            SoundManager.PlaySoundUI(panelEnableSound);
            if (Time.timeScale != 0)
            {
                timeScale = Time.timeScale;
            }
            Time.timeScale = 0f;

            if (minigameController.currentMinigame != null)
            {
                minigameController.currentMinigame.StopMinigame();
            }

        }
        else
        {
            pausePanel.SetActive(false);
            SoundManager.PlaySoundUI(panelDisableSound);
            if (minigameController.currentMinigame != null)
            {
                minigameController.currentMinigame.ContinueMinigame();
            }
            else
            {
                Time.timeScale = timeScale;
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {

        if (pause)
        {
            isPaused = false;
            PauseOrResume();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            isPaused = false;
            PauseOrResume();
        }
    }
}
