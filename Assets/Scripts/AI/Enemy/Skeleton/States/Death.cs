using System.Collections;
using UnityEngine;

namespace Skeleton.State
{
    public class Death : IState
    {
        private Skeleton _skeleton;
        private Animator _animator;
        private Collider _collider;

        private static readonly int s_death = Animator.StringToHash("IsDead");

        public Death(Skeleton skeleton, Animator animator, Collider collider)
        {
            this._skeleton = skeleton;
            this._animator = animator;
            this._collider = collider;
        }

        public void OnEnter()
        {
            Debug.Log("Skeleton is dead!");
            _animator.SetBool(s_death, true);
            _collider.enabled = false;
            _skeleton.StartCoroutine(DieAfterDeathAnimation());
        }

        public void OnExit()
        {
            // there is no exit my boi...
        }

        public void Tick()
        {
        }

        private IEnumerator DieAfterDeathAnimation()
        {
            // disappears after 3 seconds... i guess
            yield return new WaitForSeconds(3f);
            _skeleton.Die();
        }

    }
}
