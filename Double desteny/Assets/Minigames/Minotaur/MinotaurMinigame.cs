using Assets.Mappers;
using Assets.Minigames;
using Assets.Minigames.Minotaur;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinotaurMinigame : Minigame
{
    public override string Name => MinigamesNames.Minotaur;

    public MinotaurCharacter character;
    public MinotaurEnemy enemy;
    public MinotaurStick stick;
    public MazeGenerator mazeGenerator;
    public GameObject winText;
    public Animator animator;

    private float stickGuidTime = 2f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;

    private void Start()
    {
        
        //StartMinigame();
    }

    public override void WinMinigame()
    {
        base.WinMinigame();
        gameResult = MinigameResult.Win;
        winText.SetActive(true);
        animator.Play("CloseMinotaur", 0);
    }

    public override void LooseMinigame()
    {
        base.LooseMinigame();
        gameResult = MinigameResult.Loose;
        animator.Play("CloseMinotaur", 0);
    }

    public override void StopMinigame()
    {
        base.StopMinigame();
        character.StopMoving();
        enemy.StopMoving();
    }

    public override void ContinueMinigame()
    {
        base.ContinueMinigame();
        character.StartMoving();
        enemy.StartMoving();
    }

    public override void StartMinigame()
    {
        base.StartMinigame();
        gameObject.SetActive(true);
        winText.SetActive(false);
        animator.Play("Minotaur", 0);
        character.SetupChar();
        enemy.SetupEnemy();
        mazeGenerator.CreateMaze();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        stick.Disable();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameResult == MinigameResult.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = Input.mousePosition;
                stick.SetPosition(firstPressPos);
                if (GuidMapper.GetStageStatus(GuidStages.Minotaur) == GuidStatus.WhaitingForActivation)
                {
                    GuidManager.Instance.SkipWithDelay(stickGuidTime);
                }
            }
            if (Input.GetMouseButton(0))
            {
                //save ended touch 2d point
                secondPressPos = Input.mousePosition;

                stick.movingVector = secondPressPos;
                character.movingVector = stick.localPoint;
            }

            if (Input.GetMouseButtonUp(0))
            {
                stick.Disable();
                character.movingVector = Vector2.zero;
            }
        }
    }

    internal CellView CalculateNextPoint()
    {
        var list = mazeGenerator.FindPath(enemy.currentCell, character.currentCell).ToList();
        if (enemy.currentCell != character.currentCell)
        {
            return list[list.Count - 3];
        }
        return enemy.currentCell;
    }
}
