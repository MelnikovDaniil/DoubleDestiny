using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration,float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            if (Time.timeScale != 0)
            {
                var coof = 1.00f - (float)elapsed / duration;
                float x = Random.Range(-1, 1) * magnitude * coof;
                float y = Random.Range(-1, 1) * magnitude * coof;
                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
