using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public int EnemyHealth = 100;

    public Animator enemyAnimator;

    //Navmesh
    public NavMeshAgent enemyAgent;
    public Transform player;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    //Rotation Bug Fix
    public Transform enemy;
    public float fixedRotation = 0;

    //Patrolling
    public Vector3 walkPointOrigin;
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;


    //Detect and chasing
    public float sightRange, attackRange;
    public bool EnemySightRange, EnemyAttackRange;

    //Attacking
    public float attackDelay;
    public bool isAttacking;
    

    void Start()
    {
        enemyAgent= GetComponent<NavMeshAgent>();
        enemy = transform;
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        EnemySightRange = Physics.CheckSphere(transform.position,sightRange,playerLayer);
        EnemyAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!EnemySightRange && !EnemyAttackRange)
        {
            Patroll();
            enemyAnimator.SetBool("Patroll",true);
            enemyAnimator.SetBool("PlayerDetect",false);
            enemyAnimator.SetBool("PlayerAttack",false);
        }
        else if (EnemySightRange && !EnemyAttackRange)
        {
            DetectPlayer();
            enemyAnimator.SetBool("Patroll", false);
            enemyAnimator.SetBool("PlayerDetect", true);
            enemyAnimator.SetBool("PlayerAttack", false);
        }
        else if (EnemySightRange && EnemyAttackRange)
        {
            AttackPlayer();
            enemyAnimator.SetBool("Patroll", false);
            enemyAnimator.SetBool("PlayerDetect", false);
            enemyAnimator.SetBool("PlayerAttack", true);
        }


        enemy.eulerAngles = new Vector3(fixedRotation ,enemy.eulerAngles.y,enemy.eulerAngles.z);
    }

    void Patroll()
    {
        
        enemyAgent.isStopped = false;
        if (walkPointSet == false)
        {
            float randomZPos = Random.Range(-walkPointRange,walkPointRange);
            float randomXPos = Random.Range(-walkPointRange,walkPointRange);

            walkPointOrigin = new Vector3(enemyAgent.transform.position.x + randomXPos, 1, enemyAgent.transform.position.z + randomZPos);

            RaycastHit hit;
            if (Physics.Raycast(walkPointOrigin,Vector3.down,out hit,6, LayerMask.GetMask("Ground")))
            {
                walkPoint = hit.point;
                walkPointSet = true;
            }

        }

        enemyAgent.speed = 2f;

        if (walkPointSet == true)
        {
            enemyAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = enemyAgent.transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void DetectPlayer()
    {
        enemyAgent.isStopped = false;
        enemyAgent.SetDestination(player.position);
        transform.LookAt(player);
        enemyAgent.speed = 10f;
    }

    void AttackPlayer()
    {
        enemyAgent.SetDestination(player.position);
        transform.LookAt(player);

        isAttacking = true;
        enemyAgent.isStopped = true;
    }


    public void EnemyTakeDamage(int damageAmount)
    {
        EnemyHealth -= damageAmount;
        if (EnemyHealth <= damageAmount)
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
