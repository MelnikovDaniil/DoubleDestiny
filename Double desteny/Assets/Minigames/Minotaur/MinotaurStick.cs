using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.Minotaur
{

    public class MinotaurStick : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Vector2 movingVector;
        public Image stick;
        public Vector2 localPoint;
        public float maxMagnitude;

        private float magnitude;
        private Vector2 startPoint;
        private Vector2 secondPoint;


        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               rectTransform,
               movingVector,
               GetComponentInParent<Canvas>().worldCamera,
               out secondPoint);

            magnitude = Vector2.Distance(rectTransform.position, secondPoint);
            if (magnitude <= maxMagnitude)
            {
                localPoint = secondPoint - (Vector2)rectTransform.position;
                localPoint.Normalize();
                localPoint *= magnitude / maxMagnitude;
                stick.rectTransform.anchoredPosition = secondPoint;

            }
            else
            {
                localPoint = secondPoint - (Vector2)rectTransform.position;
                if (localPoint.magnitude > maxMagnitude * 2)
                {
                    localPoint.Normalize();
                    rectTransform.anchoredPosition += localPoint * 3;
                }
                localPoint.Normalize();
                stick.rectTransform.anchoredPosition = localPoint * maxMagnitude;
                
            }
        }

        public void SetPosition(Vector2 vector)
        {
            rectTransform.position = Vector2.zero;
            stick.rectTransform.position = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                vector,
                GetComponentInParent<Canvas>().worldCamera,
                out startPoint);
            rectTransform.anchoredPosition = startPoint;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
