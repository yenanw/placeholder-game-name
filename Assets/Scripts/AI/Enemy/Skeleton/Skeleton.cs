using System;
using UnityEngine;
using UnityEngine.AI;

using Skeleton.State;

namespace Skeleton
{
    public class Skeleton : MonoBehaviour
    {
        public Transform Target;
        public LayerMask GroundMask, TargetMask;
        public float SightRange, AttackRange, WalkRange;
        public float RotateSpeed;
        public float RunSpeed, WalkSpeed;

        private NavMeshAgent _agent;
        private Animator _animator;
        private StateMachine _stateMachine;


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();

            // init all states
            var wanderingAndIdling = new WanderingAndIdling(this, _agent, _animator);
            var chasingPlayer = new ChasingPlayer(this, _agent, _animator);
            var attackingPlayer = new AttackingPlayer(this, _animator);

            // init predicates
            Func<bool> PlayerInAttackRange() => () => Physics.CheckSphere(transform.position, AttackRange, TargetMask);
            Func<bool> PlayerInSightRange() => () => Physics.CheckSphere(transform.position, SightRange, TargetMask);

            // setup the transistions
            _stateMachine.AddTransition(wanderingAndIdling, chasingPlayer, PlayerInSightRange());
            _stateMachine.AddTransition(chasingPlayer, attackingPlayer, PlayerInAttackRange());
            _stateMachine.AddTransition(chasingPlayer, wanderingAndIdling, () => !PlayerInSightRange().Invoke());
            _stateMachine.AddTransition(attackingPlayer, wanderingAndIdling, () => !attackingPlayer.IsAttacking() && !PlayerInSightRange().Invoke());
            _stateMachine.AddTransition(attackingPlayer, chasingPlayer, () => !attackingPlayer.IsAttacking() && !PlayerInAttackRange().Invoke());

            _stateMachine.SetState(wanderingAndIdling);
        }


        private void Update() => _stateMachine.Tick();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SightRange);
        }
    }
}