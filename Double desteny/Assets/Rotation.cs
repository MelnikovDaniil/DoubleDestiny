using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public List<Sprite> list;
    private void Start()
    {
        System.Random random = new System.Random();
        GetComponent<Image>().sprite = list[random.Next(0, list.Count)];
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 10));
    }
}
