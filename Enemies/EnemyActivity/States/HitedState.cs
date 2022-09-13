using UnityEngine;

namespace Enemies.States
{
    public class HitedState : IState
    {
        private bool isFinished;
        public bool IsFinished => isFinished;
        public ActivityType ActivityType => ActivityType.Hit;
        private readonly Animator animator;
      
        public HitedState(Animator animator)
        {
            this.animator = animator;
        }
        
        public void EnableState()
        {
           animator.SetTrigger("Hit");
        }

        public void Tick()
        {
            float animationTime = Utils.Utils.GetCurrentAnimatorTime(animator);
            if (animationTime >.9 )
            {
                isFinished = true;
            }
        }
        
        public void DisableState()
        {
            
        }
    }
}