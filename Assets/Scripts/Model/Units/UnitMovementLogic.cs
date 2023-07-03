using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Model
{
    public class UnitMovementLogic: MonoBehaviour
    {
        private Queue<Vector2> targetsQueue;
        
        private bool isMoving;
        private float movementProgress;
        
        private Vector2 startPosition;
        private Vector2 currentTarget;
        
        
        private void Awake()
        {
            targetsQueue = new Queue<Vector2>();
        }

        private void Update()
        {
            if (!isMoving && targetsQueue.Count != 0)
            {
                startPosition = transform.position;
                currentTarget = targetsQueue.Dequeue();
                movementProgress = 0;
                isMoving = true;
            }

            if (isMoving)
            {
                movementProgress += Time.deltaTime;
                if (movementProgress < 1)
                    transform.position = Vector2.Lerp(startPosition, currentTarget, movementProgress);
                else
                {
                    transform.position = currentTarget;
                    isMoving = false;
                }
            }
        }

        public void MoveTo(Vector2 targetPosition)
        {
            targetsQueue.Enqueue(targetPosition);
        }
    }
}