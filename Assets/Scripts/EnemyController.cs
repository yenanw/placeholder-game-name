using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public NavMeshAgent agent;


    public float sightRange, attackRange;
    public float walkPointRange;

    public LayerMask groundMask, playerMask;


    private Transform target;
    private Vector3 walkPoint;
    private bool walkPointSet;

    private void Start()
    {
        target = PlayerManager.instance.GetPlayer().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool inSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        bool inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (inAttackRange)
            Attack();
        else if (inSightRange)
            Chase();
        else
            Patrol();
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        FaceTarget();


        // attack code here or something...
    }

    private void Chase()
    {   
        walkPointSet = false;
        agent.SetDestination(target.position);
    }

    private void Patrol()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        
        agent.SetDestination(walkPoint);

        Vector3 distance = transform.position - walkPoint;

        if (distance.magnitude < 1f)
            walkPointSet = false;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
