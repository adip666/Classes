using UnityEngine;
using Zenject;

namespace Enemies.States
{
    public interface IState : ITickable
    {
        void EnableState();
        bool IsFinished { get; }
        
        ActivityType ActivityType { get; }
        void DisableState();
    }
}