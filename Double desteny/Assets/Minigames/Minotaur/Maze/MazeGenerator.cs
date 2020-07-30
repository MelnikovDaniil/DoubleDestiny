using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public MazeCell[,] mazeCells;
    public CellView cellPrefub;
    public List<CellView> cellVeiws = new List<CellView>();
    public float distanceBetweenCells;
    public GridLayoutGroup grid;

    public Sprite doorSprite;

    public void CreateMaze()
    {
        foreach (var cell in cellVeiws)
        {
            Destroy(cell.gameObject);
        }
        cellVeiws.Clear();

        grid.cellSize = new Vector2(distanceBetweenCells, distanceBetweenCells);
        grid.constraintCount = width;

        GenerateMaze();

        CellView cellBuffer;
        foreach (var cell in mazeCells)
        {
            cellBuffer = Instantiate(cellPrefub, transform);

            cellBuffer.x = cell.x;
            cellBuffer.y = cell.y;
            cellBuffer.distanceFromStart = cell.distanceFromStart;

            //cellBuffer.rectTransform.anchoredPosition = 
            //    new Vector2(
            //        cell.x * distanceBetweenCells,
            //        cell.y * distanceBetweenCells);
            cellBuffer.bottomWall.SetActive(cell.bottomWall);
            cellBuffer.leftWall.SetActive(cell.leftWall);
            cellVeiws.Add(cellBuffer);
        }

        foreach (var cellView in cellVeiws.Where(x => x.y == height - 1))
        {
            cellView.groundImage.enabled = false;
            cellView.leftWall.SetActive(false);
        }

        foreach (var cellView in cellVeiws.Where(x => x.x == width - 1))
        {
            cellView.groundImage.enabled = false;
            cellView.bottomWall.SetActive(false);
        }

        var startCell = cellVeiws.First(cell => cell.x == 0 && cell.y == height / 2);
        startCell.leftWall.SetActive(false);
        startCell.leftDoor.SetActive(true);
        PlaceExit();
    }

    public void GenerateMaze()
    {
        mazeCells = new MazeCell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mazeCells[x, y] = new MazeCell
                {
                    x = x,
                    y = y,
                    leftWall = true,
                    bottomWall = true,
                };
            }
        }
        RemoveWallsWithBackTracker();

    }

    private void PlaceExit()
    {
        var longestWayCell = cellVeiws.First(cell => cell.x == 0 && cell.y == height / 2);
        for (int i = 0; i < height - 1; i++)
        {
            if (longestWayCell.distanceFromStart < cellVeiws.First(x => x.x == width - 2 && x.y == i).distanceFromStart)
            {
                longestWayCell = cellVeiws.First(x => x.x == width - 2 && x.y == i);
            }
        }

        longestWayCell.groundImage.sprite = doorSprite;
        longestWayCell.winPoint.SetActive(true);
    }

    private void RemoveWallsWithBackTracker()
    {
        var currentCell = mazeCells[0, height / 2];
        currentCell.Visited = true;
        currentCell.distanceFromStart = 0;

        var stack = new Stack<MazeCell>();

        do
        {
            var x = currentCell.x;
            var y = currentCell.y;

            var unvisitedCells = new List<MazeCell>();
            if (currentCell.x > 0 && !mazeCells[x - 1, y].Visited)
                unvisitedCells.Add(mazeCells[x - 1, y]);
            if (currentCell.y > 0 && !mazeCells[x, y - 1].Visited)
                unvisitedCells.Add(mazeCells[x, y - 1]);
            if (currentCell.x < width - 2 && !mazeCells[x + 1, y].Visited)
                unvisitedCells.Add(mazeCells[x + 1, y]);
            if (currentCell.y < height - 2&& !mazeCells[x, y + 1].Visited)
                unvisitedCells.Add(mazeCells[x, y + 1]);

            if (unvisitedCells.Count > 0)
            {
                var chosen = unvisitedCells[UnityEngine.Random.Range(0, unvisitedCells.Count)];
                RemoveWall(currentCell, chosen);
                chosen.Visited = true;
                chosen.distanceFromStart = stack.Count;
                currentCell = chosen;
                stack.Push(chosen);

            }
            else
            {
                currentCell = stack.Pop();
            }
        } while (stack.Count > 0);
    }


    public Stack<CellView> FindPath(CellView minotaurCell, CellView characterCell)
    {
        foreach (var item in cellVeiws)
        {
            item.VisitByEnemy = false;
        }

        var currentCell = minotaurCell; //dasdasdadsa
        currentCell.VisitByEnemy = true;
        currentCell.distanceFromStart = 0;
        var stack = new Stack<CellView>();
        stack.Push(currentCell);
        do
        {
            var x = currentCell.x;
            var y = currentCell.y;

            var unvisitedCells = new List<CellView>();
            if (currentCell.x > 0
                && !cellVeiws.First(cell => cell.x == x - 1 && cell.y == y).VisitByEnemy
                && !currentCell.leftWall.activeSelf)
            {
                unvisitedCells.Add(cellVeiws.First(cell => cell.x == x - 1 && cell.y == y));
            }

            if (currentCell.y > 0 
                && !cellVeiws.First(cell => cell.x == x && cell.y == y - 1).VisitByEnemy
                && !currentCell.bottomWall.activeSelf)
            {
                unvisitedCells.Add(cellVeiws.First(cell => cell.x == x && cell.y == y - 1));
            }

            var rightCell = cellVeiws.FirstOrDefault(cell => cell.x == x + 1 && cell.y == y);
            if (rightCell != null
                && !rightCell.VisitByEnemy
                && !rightCell.leftWall.activeSelf)
            {
                unvisitedCells.Add(rightCell);
            }

            var topCell = cellVeiws.FirstOrDefault(cell => cell.x == x && cell.y == y + 1);
            if (topCell != null
                && !topCell.VisitByEnemy
                && !topCell.bottomWall.activeSelf)
            {
                unvisitedCells.Add(topCell);
            }

            if (unvisitedCells.Count > 0)
            {
                stack.Push(currentCell);
                var chosen = unvisitedCells.First();
                chosen.VisitByEnemy = true;
                chosen.distanceFromStart = stack.Count;
                currentCell = chosen;
                stack.Push(chosen);
            }
            else
            {
                currentCell = stack.Pop();
            }
        } while (currentCell != characterCell);

        //foreach (var item in stack)
        //{
        //    item.groundImage.color = Color.yellow;
        //}

        return stack;
    }

    private void RemoveWall(MazeCell a, MazeCell b)
    {
        if (a.x == b.x)
        {
            if(a.y > b.y)
            {
                a.bottomWall = false;
            }
            else
            {
                b.bottomWall = false;
            }
        }

        if (a.y == b.y)
        {
            if (a.x > b.x)
            {
                a.leftWall = false;
            }
            else
            {
                b.leftWall = false;
            }
        }
    }
}
