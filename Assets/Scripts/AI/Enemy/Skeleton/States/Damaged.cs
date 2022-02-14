using UnityEngine;

namespace Skeleton.State
{
    public class Damaged : IState
    {
        private Animator _animator;

        private bool _isDamagedPlaying = true;

        private static readonly int s_damaged = Animator.StringToHash("Damaged");

        public Damaged(Animator animator)
        {
            this._animator = animator;
        }

        public void OnEnter()
        {
            Debug.Log("Skeleton got hit!");
            _animator.SetTrigger(s_damaged);
        }

        public void OnExit()
        {
            _animator.ResetTrigger(s_damaged);
        }

        public bool IsDamagedPlaying()
        {
            return !_isDamagedPlaying;
        }

        public void Tick()
        {
            _isDamagedPlaying = _animator.GetBool(s_damaged);
        }

        
    }
}
