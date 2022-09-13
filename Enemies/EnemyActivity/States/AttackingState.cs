using Enemies.Settings;
using Player;
using UnityEngine;

namespace Enemies.States
{
    public class AttackingState : IState
    {
        private readonly Transform attackPoint;
        private readonly Animator animator;
        private readonly IEnemySettings enemySettings;
        public ActivityType ActivityType => ActivityType.Attack;
        
        private bool isFinished;
        public bool IsFinished => isFinished;

        private bool canAttack = true;
        private float currentAttackTime;
        
        public AttackingState(Transform attackPoint ,Animator animator, IEnemySettings enemySettings)
        {
            this.attackPoint = attackPoint;
            this.animator = animator;
            this.enemySettings = enemySettings;
        }
        public void EnableState()
        {
            canAttack = true;
            currentAttackTime = 0;
        }
        
        public void DisableState()
        {
        }

        public void Tick()
        {
            if (canAttack)
            {
                canAttack = false;
                currentAttackTime = 0;
                animator.SetTrigger("Attack");
                Collider2D[] hitElements =
                    Physics2D.OverlapCircleAll(attackPoint.position, enemySettings.AttackRange);

                foreach (Collider2D hitElement in hitElements)
                {
                    if (hitElement.GetComponent<PlayerSystem>())
                    {
                        hitElement.GetComponent<PlayerSystem>().AddDamage(Vector3.zero);
                        break;
                    }
                   
                }
            }

            if (!canAttack)
            {
                currentAttackTime += Time.deltaTime;
                if (currentAttackTime > enemySettings.AttackTimeOffset)
                {
                    canAttack = true;
                    currentAttackTime = 0;
                }
            }
        }
    }
}