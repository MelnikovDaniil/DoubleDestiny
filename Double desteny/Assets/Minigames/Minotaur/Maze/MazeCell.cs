using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public int x;
    public int y;

    public bool Visited;
    public bool leftWall;
    public bool bottomWall;

    public int distanceFromStart;
}
