using UnityEngine;

namespace Enemies.States
{
    public class DyingState : IState
    {
        private bool isFinished;
        public bool IsFinished => isFinished;
        public ActivityType ActivityType => ActivityType.Die;
        
        private readonly Animator animator;
        
        public DyingState(Animator animator)
        {
            this.animator = animator;
        }
        
        public void Tick()
        {
            float animationTime = Utils.Utils.GetCurrentAnimatorTime(animator);
            if (animationTime >.9 )
            {
                isFinished = true;
            }
        }

        public void EnableState()
        {
            animator.SetTrigger("Die");
        }
        
        public void DisableState()
        {
            
        }
    }
}