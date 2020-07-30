using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public float offsetY;
    public float offsetX;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        CopyComponent(GetComponent<Animator>(), gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + new Vector3(offsetX, offsetY, 0);
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}
