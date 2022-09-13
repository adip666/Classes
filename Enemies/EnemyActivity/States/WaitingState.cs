using UnityEngine;

namespace Enemies.States
{
    public class WaitingState : IState
    {
        private bool isFinished;
        public bool IsFinished => isFinished;
        public ActivityType ActivityType => ActivityType.Waiting;

        private readonly Animator animator;
        public WaitingState(Animator animator)
        {
            this.animator = animator;
        }

        public void Tick()
        {
            
        }

        public void EnableState()
        {
            animator.SetBool("Wait",true);
        }

        public void DisableState()
        {
            animator.SetBool("Wait",false);

        }
    }
}