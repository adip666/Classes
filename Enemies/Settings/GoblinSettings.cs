using System;
using UnityEngine;

namespace Enemies.Settings
{
    [Serializable]
    public class GoblinSettings : IEnemySettings
    {
        [SerializeField] int movementSpeed = 1;
        [SerializeField] private float attackTimeOffset = 1;
        [SerializeField] private int life = 1;
        [SerializeField] private float attackRange = .5f;
        public int MovementSpeed => movementSpeed;
        public int Life => life;
        public float AttackTimeOffset => attackTimeOffset;

        public float AttackRange => attackRange;
    }
}