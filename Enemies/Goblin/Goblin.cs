using System;
using System.Collections.Generic;
using Enemies.States;
using UnityEngine;

namespace Enemies
{
    public class Goblin : Enemy
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform attackPoint;
      
        public override void Initialize()
        {
            base.Initialize();
            CreateActivityStates();
            ChangeToDefaultActivity();
        }

        protected override void ChangeToDefaultActivity()
        {
            IState startActivity = patrolPoints.Count == 0
                ? GetActivity(ActivityType.Waiting)
                : GetActivity(ActivityType.Patrol);
            ChangeActivity(startActivity);
        }

        private void CreateActivityStates()
        {
            activity = new List<IState>();
            activity.Add(new WaitingState(animator));
            activity.Add(new PatrollingState(transform, animator, patrolPoints, enemySettings));
            activity.Add(new AttackingState(attackPoint, animator, enemySettings));
            activity.Add(new HitedState(animator));
            activity.Add(new DyingState(animator));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (CurrentActivity.ActivityType == ActivityType.Attack)
                    return;
                ChangeActivity(GetActivity(ActivityType.Attack));
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ChangeToDefaultActivity();
            }
        }
    }
}