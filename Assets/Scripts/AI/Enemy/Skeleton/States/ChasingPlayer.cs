using UnityEngine;
using UnityEngine.AI;

namespace Skeleton.State
{
    public class ChasingPlayer : IState
    {

        private readonly Skeleton _skeleton;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;

        private static readonly int s_speed = Animator.StringToHash("Speed");

        public ChasingPlayer(Skeleton skeleton, NavMeshAgent agent, Animator animator)
        {
            this._skeleton = skeleton;
            this._agent = agent;
            this._animator = animator;
        }
        public void OnEnter()
        {
            Debug.Log("Skeleton is chasing the player.");
            _agent.enabled = true;
            _agent.speed = _skeleton.RunSpeed;
            _animator.SetFloat(s_speed, _skeleton.RunSpeed);
        }

        public void OnExit()
        {
            _agent.enabled = false;
            _animator.SetFloat(s_speed, 0f);
        }

        public void Tick()
        {
            _agent.SetDestination(_skeleton.Target.transform.position);
        }

    }
}
