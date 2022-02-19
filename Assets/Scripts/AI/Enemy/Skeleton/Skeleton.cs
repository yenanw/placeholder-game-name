using System;
using UnityEngine;
using UnityEngine.AI;

using Skeleton.State;

namespace Skeleton
{
    public class Skeleton : MonoBehaviour, IDamageable
    {
        public Transform Target;
        public LayerMask GroundMask, TargetMask;

        public float SightRange, AttackRange, WalkRange;
        public float RotateSpeed;
        public float RunSpeed, WalkSpeed;
        
        public HealthBar HealthBar;
        public float Health { get; set; }

        private NavMeshAgent _agent;
        private Animator _animator;
        private Collider _collider;
        private StateMachine _stateMachine;

        private bool _isDamaged = false;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();

            _stateMachine = new StateMachine();

            // init all states
            var wanderingAndIdling = new WanderingAndIdling(this, _agent, _animator);
            var chasingPlayer = new ChasingPlayer(this, _agent, _animator);
            var attackingPlayer = new AttackingPlayer(this, _animator);
            var damaged = new Damaged(_animator);
            var death = new Death(this, _animator, _collider);

            // init predicates
            Func<bool> PlayerInAttackRange() => () => Physics.CheckSphere(transform.position, AttackRange, TargetMask);
            Func<bool> PlayerInSightRange() => () => Physics.CheckSphere(transform.position, SightRange, TargetMask);
            Func<bool> DidGetDamaged() => () =>
            {
                if (_isDamaged)
                {
                    _isDamaged = false;
                    return true;
                }
                return false;
            };

            // setup the transistions
            _stateMachine.AddTransition(wanderingAndIdling, chasingPlayer, PlayerInSightRange());
            _stateMachine.AddTransition(chasingPlayer, attackingPlayer, PlayerInAttackRange());
            _stateMachine.AddTransition(chasingPlayer, wanderingAndIdling, () => !PlayerInSightRange().Invoke());
            _stateMachine.AddTransition(attackingPlayer, wanderingAndIdling, () => !attackingPlayer.IsAttacking() && !PlayerInSightRange().Invoke());
            _stateMachine.AddTransition(attackingPlayer, chasingPlayer, () => !attackingPlayer.IsAttacking() && !PlayerInAttackRange().Invoke());
            _stateMachine.AddTransition(damaged, attackingPlayer, () => !damaged.IsDamagedPlaying() && PlayerInAttackRange().Invoke());
            _stateMachine.AddTransition(damaged, chasingPlayer, () => !damaged.IsDamagedPlaying() && PlayerInSightRange().Invoke() && !PlayerInSightRange().Invoke());
            _stateMachine.AddTransition(damaged, wanderingAndIdling, () => !damaged.IsDamagedPlaying() && !PlayerInSightRange().Invoke());

            _stateMachine.AddAnyTransition(damaged, DidGetDamaged());
            // not outgoing state from death, death is inevitable
            _stateMachine.AddAnyTransition(death, () => this.Health <= 0f);

            _stateMachine.SetState(wanderingAndIdling);

            // other init
            Health = 100f;
            HealthBar.SetUp(Health);
        }


        private void Update() => _stateMachine.Tick();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SightRange);
        }

        public void Damage(float dmg)
        {
            Health -= dmg;
            _isDamaged = true;
            HealthBar.SetHealth(Health);
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}