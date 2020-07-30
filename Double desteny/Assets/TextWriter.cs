using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    //public GridLayoutGroup textArea;
    //public Vector2 liaterSize = new Vector2(12, 24);
    //public Vector2 spaceBetween = new Vector2(0, 0);
    public float lineHeight = 40;
    public float lineLetterCount = 10;
    public float typingTimeRate = 0.1f;

    public float shakingDuration = 3;
    public float shakingMagnitude = 0.5f;
    public RectTransform dialogWindow;
    public Text textArea;

    public float distanceY = 120;
    public float distanceX = 60;

    public void TypeText(string text)
    {
        StopAllCoroutines();
        dialogWindow.sizeDelta = new Vector2(dialogWindow.sizeDelta.x, 1000);
        textArea.text = text;
        Canvas.ForceUpdateCanvases();
        dialogWindow.sizeDelta = new Vector2(dialogWindow.sizeDelta.x, lineHeight * textArea.cachedTextGenerator.lineCount);
        textArea.text = string.Empty;
        StartCoroutine(TypeTextCoroutine(text));
    }

    public void StartShaking()
    {
        Debug.Log("Shake");
        StartCoroutine(ShakeText(shakingDuration, shakingMagnitude));
    }

    private IEnumerator TypeTextCoroutine(string text)
    {
        foreach (var leater in text)
        {
            textArea.text += leater;
            yield return new WaitForSecondsRealtime(typingTimeRate);
        }
    }

    public IEnumerator ShakeText(float duration, float magnitude)
    {
        Vector3 originalPos = textArea.rectTransform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            if (Time.timeScale != 0)
            {
                var coof = 1.00f - (float)elapsed / duration;
                float x = Random.Range(-1, 1) * magnitude * coof;
                float y = Random.Range(-1, 1) * magnitude * coof;
                textArea.rectTransform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
        textArea.rectTransform.localPosition = originalPos;
    }

    public void MoveDialogDown()
    {
        dialogWindow.anchoredPosition = new Vector2(-distanceX, -distanceY);
    }

    public void MoveDialogUp()
    {
        dialogWindow.anchoredPosition = new Vector2(-distanceX, distanceY);
    }

    public void SetStandartPosition()
    {
        dialogWindow.anchoredPosition = Vector2.zero;
    }
}
