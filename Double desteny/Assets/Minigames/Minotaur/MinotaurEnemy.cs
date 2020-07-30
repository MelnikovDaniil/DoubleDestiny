using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Minigames.Minotaur
{
    public class MinotaurEnemy : MonoBehaviour
    {
        public Transform target;
        public float speed;
        public CellView currentCell;
        public CellView nextCell;
        public MinotaurMinigame minigame;
        public RectTransform rectTransform;
        public Vector2 MovingVector;
        private Collider2D foundCollider;
        private bool isInvoke;
        private float currentSpeed;
        private Animator _animator;

        private readonly Vector2 startPosition = new Vector2(-180, 16);

        public void StopMoving()
        {
            currentSpeed = 0;
            _animator.speed = 0;
        }

        public void StartMoving()
        {
            currentSpeed = speed;
            _animator.speed = 1;
        }

        public void SetupEnemy()
        {
            rectTransform.anchoredPosition = startPosition;
            MovingVector = Vector2.right;
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            _animator.enabled = true;
        }

        public void Run()
        {
            _animator.enabled = false;
            foundCollider = GetComponent<CircleCollider2D>();
            currentSpeed = speed;
        }

        void Update()
        {
            rectTransform.anchoredPosition += MovingVector * currentSpeed;
            if (MovingVector == Vector2.zero && !isInvoke)
            {
                isInvoke = true;
                CalculateNextPoint(); 
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        { 
            if (collision.tag == "cell")
            {
                var cell = collision.GetComponent<CellView>();
                currentCell = cell;
                nextCell = minigame.CalculateNextPoint();
                MovingVector = new Vector2(nextCell.x - currentCell.x, nextCell.y - currentCell.y);
            }
        }

        public void CalculateNextPoint()
        {
            isInvoke = false;
            nextCell = minigame.CalculateNextPoint();
            MovingVector = new Vector2(nextCell.x - currentCell.x, nextCell.y - currentCell.y);
        }
    }
}
