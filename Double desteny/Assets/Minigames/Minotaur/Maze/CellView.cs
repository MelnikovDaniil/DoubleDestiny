using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject leftWall;
    public GameObject bottomWall;
    public GameObject leftDoor;
    public GameObject winPoint;
    public Image groundImage;
    public int x;
    public int y;
    public int distanceFromStart;
    public bool VisitByEnemy;
}