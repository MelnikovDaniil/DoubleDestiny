using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMageSkill : MonoBehaviour
{
    public Transform toObj;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        Invoke("Move", 0.1f);
    }

    public void Move()
    {
        transform.position = new Vector3(toObj.position.x, toObj.position.y + 1f, toObj.position.z);
    }
}
