using Assets.Minigames;
using Assets.Minigames.EventCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour
{
    public List<Minigame> minigames;

    public GameEvent currentGameEvent;
    [NonSerialized]
    public Minigame currentMinigame;

    public Animator UIAnimator;
    public GameObject leftPanel, rightPanel;
    private Scene scene;
    private PhysicsScene2D scenePhisyc;
    public CharactersScript characters;
    public ParticleSystem leaveParticles;
    public SturtUp startUp;
    private float unscaleDeltaTime;

    private void Start()
    {
        unscaleDeltaTime = Time.unscaledDeltaTime;
        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        LoadSceneParameters lsp = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics2D);
        scene = SceneManager.CreateScene("MinigameScene", csp);
        scenePhisyc = scene.GetPhysicsScene2D();
        var list = SceneManager.GetSceneByName("MinigameScene").GetRootGameObjects();
        SceneManager.MoveGameObjectToScene(gameObject, scene);
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }

    private void Update()
    {
        scenePhisyc.Simulate(unscaleDeltaTime);
    }

    public void StartMinigame(string name, GameEvent gameEvent)
    {
        var particles = leaveParticles.main;
        particles.useUnscaledTime = true;
        currentGameEvent = gameEvent;
        var minigame = minigames.First(x => x.Name == name);
        var currentCharacter = characters.wariorBl ? 
                characters.warior.GetComponent<Char>().icon
                : characters.ranger.GetComponent<Char>().icon;
        minigame.CharacterSprite = currentCharacter;
        minigame.StartMinigame();
        currentMinigame = minigame;

        UIAnimator.Play("GamePanel", 0);
        leftPanel.SetActive(false);
        rightPanel.SetActive(false);
    }

    public void WinCurrentEvent()
    {
        var particles = leaveParticles.main;
        particles.useUnscaledTime = false;
        currentGameEvent.WinCurrentEvent();
        currentGameEvent = null;
        currentMinigame = null;
        UIAnimator.Play("GamePanelReturn", 0);
        leftPanel.SetActive(true);
        rightPanel.SetActive(true);
        startUp.PlayMusic();
    }

    public void LooseCurrentEvent()
    {
        var particles = leaveParticles.main;
        particles.useUnscaledTime = false;
        currentGameEvent.LooseCurrentEvent();
        currentGameEvent = null;
        currentMinigame = null;
        UIAnimator.Play("GamePanelReturn", 0);
        leftPanel.SetActive(true);
        rightPanel.SetActive(true);
        startUp.PlayMusic();
    }
}
