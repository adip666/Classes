using System;
using System.Collections.Generic;
using Enemies.Settings;
using Enemies.States;
using Player;
using Signals;
using SignalsSystem;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IFixedTickable
    {
        [Inject] private ISignalSystem signalSystem;
        [Inject] protected IEnemySettings enemySettings;

        protected List<Transform> patrolPoints = new List<Transform>();
        protected List<IState> activity;

        private IState currentActivity;
        protected IState CurrentActivity => currentActivity;
        private int currentLife;
        private bool canAddDamage = true;
        private float currentTimeToCanAddDamage;
        private float timeCanAddDamageOffset = .5f;
        public virtual void Initialize()
        {
            currentLife = enemySettings.Life;
        }

        public void SetPatrolPoints(List<Transform> patrolPoints)
        {
            this.patrolPoints = patrolPoints;
        }

        public virtual void FixedTick()
        {
            ManageActivities();
            if (!canAddDamage)
            {
                currentTimeToCanAddDamage += Time.deltaTime;
                if (currentTimeToCanAddDamage > timeCanAddDamageOffset)
                {
                    canAddDamage = true;
                    currentTimeToCanAddDamage = 0;
                }
            }
        }

        private void ManageActivities()
        {
            if (currentActivity != null)
            {
                if (!currentActivity.IsFinished)
                {
                    currentActivity.Tick();
                }
                else if (currentActivity.ActivityType == ActivityType.Die)
                {
                    signalSystem.FireSignal(new EnemyDiedSignal(this));
                }
                else
                {
                    ChangeToDefaultActivity();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && canAddDamage && currentActivity.ActivityType != ActivityType.Attack)
            {
                canAddDamage = false;
                Vector2 forceDirection = GetForceDirection(other.transform.root);
                other.transform.root.GetComponent<PlayerSystem>().AddDamage(forceDirection);
            }
        }
        

        Vector2 GetForceDirection(Transform player)
        {
            return player.transform.position.x < transform.position.x ? new Vector2(-1, 0) : new Vector2(2, 0);
        }

        public void AddDamage()
        {
            if (currentActivity.ActivityType == ActivityType.Hit)
                return;
            if (currentLife > 1)
            {
                ChangeActivity(GetActivity(ActivityType.Hit));
                currentLife--;
            }
            else
            {
                Die();
            }
        }

        protected virtual void ChangeToDefaultActivity()
        {
            ChangeActivity(GetActivity(ActivityType.Waiting));
        }

        protected IState GetActivity(ActivityType activityType)
        {
            return activity.Find(s => s.ActivityType == activityType);
        }

        protected virtual void ChangeActivity(IState state)
        {
            if (currentActivity != null)
                currentActivity.DisableState();
            state.EnableState();
            currentActivity = state;
        }

        protected virtual void Die()
        {
            ChangeActivity(GetActivity(ActivityType.Die));
        }

        public virtual void Deinitialize()
        {
        }
    }
}