using UnityEngine;
using UnityEngine.AI;

namespace Skeleton.State
{
    public class WanderingAndIdling : IState
    {
        private readonly Skeleton _skeleton;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;

        private Vector3 _destination;
        private float _idleTime = 0;
        private float _elapsedTime = 0;
        private bool _shouldIdle = false;
        private bool _isWalking = true;

        private static readonly int s_speed = Animator.StringToHash("Speed");
        private static readonly int s_idling = Animator.StringToHash("Idling");

        public WanderingAndIdling(Skeleton skeleton, NavMeshAgent agent, Animator animator)
        {
            this._skeleton = skeleton;
            this._agent = agent;
            this._animator = animator;
        }

        public void OnEnter()
        {
            Debug.Log("Skeleton is wandering.");
            _destination = SearchWalkPoint();
            _agent.enabled = true;
            _agent.SetDestination(_destination);

            _agent.speed = _skeleton.WalkSpeed;
            _animator.SetFloat(s_speed, _skeleton.WalkSpeed);
        }

        public void OnExit()
        {
            _agent.enabled = false;
            _animator.SetFloat(s_speed, 0f);
            _animator.SetBool(s_idling, false);

            _shouldIdle = false;
            _isWalking = true;
        }

        public void Tick()
        {
            if (_shouldIdle && _elapsedTime < _idleTime)
            {
                _elapsedTime += Time.deltaTime;
                return;
            }

            if (DestinationReached())
            {
                _shouldIdle = ShouldIdle();
                _isWalking = false;
            }

            if (_shouldIdle)
            {
                _idleTime = RandomIdleTime();
                _elapsedTime = 0f;
                _animator.SetFloat(s_speed, 0f);
                _animator.SetBool(s_idling, true);
                return;
            }

            if (!_isWalking)
            {
                _destination = SearchWalkPoint();
                _agent.SetDestination(_destination);
                _animator.SetBool(s_idling, false);
                _animator.SetFloat(s_speed, _skeleton.WalkSpeed);
                _isWalking = true;
            }
        }

        private bool DestinationReached()
        {
            return Vector3.Distance(_skeleton.transform.position, _destination) <= 1f;
        }

        private bool ShouldIdle()
        {
            return Random.Range(0f, 1f) >= 0.5f;
        }

        private float RandomIdleTime()
        {
            // let's say it just randomly doozes off for 2 to 6 seconds
            return Random.Range(2f, 6f);
        }

        private Vector3 SearchWalkPoint()
        {
            Vector3 RandomWalkPoint()
            {
                float range = _skeleton.WalkRange;
                float randomZ = Random.Range(-range, range);
                float randomX = Random.Range(-range, range);

                Vector3 pos = _skeleton.transform.position;
                return new Vector3(pos.x + randomX, pos.y, pos.z + randomZ);
            };

            Vector3 walkPoint = RandomWalkPoint();
            while (!Physics.Raycast(walkPoint, -_skeleton.transform.up, 2f, _skeleton.GroundMask))
            {
                walkPoint = RandomWalkPoint();
            }

            return walkPoint;
        }

    }
}
