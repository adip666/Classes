using System.Collections.Generic;
using Enemies.States;
using UnityEngine;


namespace Enemies
{
    public class Mushroom : Enemy
    {
        [SerializeField] private Animator animator;
      
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
            activity.Add(new HitedState(animator));
            activity.Add(new DyingState(animator));
        }
    }
}