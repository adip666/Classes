using System.Collections.Generic;
using Enemies.Settings;
using UnityEngine;

namespace Enemies.States
{
    public class PatrollingState : IState
    {
        private bool isFinished;
        public bool IsFinished => isFinished;
        public ActivityType ActivityType => ActivityType.Patrol;

        private readonly List<Transform> patrolsPoints;
        private readonly Animator animator;
        private readonly Transform enemyTransform;
        private readonly IEnemySettings settings;

        private int currentPatrolPointIndex = 0;
        private Transform currentPatrolTarget;
        private bool isFacingRight = true;
        public PatrollingState(Transform enemyTransform, Animator animator, List<Transform> patrolsPoints, IEnemySettings settings)
        {
            this.enemyTransform = enemyTransform;
            this.animator = animator;
            this.patrolsPoints = patrolsPoints;
            this.settings = settings;
        }
        
        public void EnableState()
        {
            animator.SetBool("Walk", true);
            currentPatrolTarget = patrolsPoints[0];
        }

        public void Tick()
        {
            Vector3 direction = GetDirection();
            Movement(direction);
            CheckPatrolPoint();
        }

        private  Vector3 GetDirection()
        {
            return enemyTransform.position.x > currentPatrolTarget.position.x
                ? new Vector3(-1, 0, 0)
                : new Vector3(1, 0, 0);
        }
        private void Movement(Vector3 direction)
        {
            enemyTransform.Translate(direction * settings.MovementSpeed * Time.deltaTime);

            if (direction.x > 0f && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0f && isFacingRight)
            {
                Flip();
            }
        }

        private void CheckPatrolPoint()
        {
           float distance = Vector3.Distance(enemyTransform.position, currentPatrolTarget.position);
           if (distance < .2f)
              currentPatrolTarget = GetNextPatrolPoint();
        }
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = enemyTransform.localScale;
            theScale.x *= -1;
            enemyTransform.localScale = theScale;
        }

        private Transform GetNextPatrolPoint()
        {
            if (currentPatrolPointIndex < patrolsPoints.Count-1)
            {
                currentPatrolPointIndex++;
            }
            else
            {
                currentPatrolPointIndex = 0;
            }
            return patrolsPoints[currentPatrolPointIndex];
        }

        public void DisableState()
        {
            animator.SetBool("Walk", false);
        }
    }
}