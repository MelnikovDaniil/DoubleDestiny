using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.Minotaur
{
    public class MinotaurCharacter : MonoBehaviour
    {
        public Vector2 movingVector;
        public RectTransform rectTransform;
        public float speed;
        public CellView currentCell;
        public Minigame minigame;

        private float currentSpeed;

        [SerializeField]
        private Image characterIcon;
        private readonly Vector2 startPosition = new Vector2(-140, 16);

        public void StopMoving()
        {
            currentSpeed = 0;
        }

        public void StartMoving()
        {
            currentSpeed = speed;
        }

        public void SetupChar()
        {
            rectTransform.anchoredPosition = startPosition;
            movingVector = Vector2.zero;
            characterIcon.sprite = minigame.CharacterSprite;
            currentSpeed = speed;
        }

        private void Update()
        {
            rectTransform.anchoredPosition += movingVector * currentSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "cell")
            {
                currentCell = collision.GetComponent<CellView>();
            }
            else if (collision.tag == "winPoint")
            {
                minigame.WinMinigame();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MinotaurEnemy>())
            {
                minigame.LooseMinigame();
            }
        }
    }
}
