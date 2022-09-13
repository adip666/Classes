using System;
using UnityEngine;

namespace Enemies.Settings
{
    [Serializable]
    public class MushroomSettings : IEnemySettings
    {
        [SerializeField] int movementSpeed = 1;
        [SerializeField] private int life;
        public int MovementSpeed => movementSpeed;
        public int Life => life;
        public float AttackTimeOffset { get; }
        public float AttackRange { get; }
    }
}