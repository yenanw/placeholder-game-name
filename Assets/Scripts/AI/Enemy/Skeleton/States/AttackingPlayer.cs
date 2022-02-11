using UnityEngine;

namespace Skeleton.State
{
    public class AttackingPlayer : IState
    {
        private Skeleton _skeleton;
        private Animator _animator;

        private Quaternion _targetRotation;

        private bool _isAttacking = false;
        private static int s_attack = Animator.StringToHash("Attack");

        public AttackingPlayer(Skeleton skeleton, Animator animator)
        {
            this._skeleton = skeleton;
            this._animator = animator;
        }

        public void OnEnter()
        {
            _animator.ResetTrigger(s_attack);
            Debug.Log("Skeleton is attacking the player");
        }

        public void OnExit()
        {
            _animator.ResetTrigger(s_attack);
            _isAttacking = false;
        }

        public void Tick()
        {
            if (!_isAttacking)
            {
                FacePlayer();
                bool isFacingTarget = Quaternion.Angle(_skeleton.transform.rotation, _targetRotation) <= 2f;
                if (!isFacingTarget)
                    return;

                _animator.SetTrigger(s_attack);
                _isAttacking = true;
                return;
            }

            _isAttacking = _animator.GetBool(s_attack);
        }

        public bool IsAttacking()
        {
            return _isAttacking;
        }

        private void FacePlayer()
        {
            Vector3 direction = (_skeleton.Target.position - _skeleton.transform.position).normalized;
            _targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _skeleton.transform.rotation = Quaternion.Slerp(_skeleton.transform.rotation,
                                                            _targetRotation,
                                                            Time.deltaTime * _skeleton.RotateSpeed);
        }

    }
}
