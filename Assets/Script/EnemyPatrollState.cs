using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class EnemyPatrollState : StateMachineBehaviour
{
    NavMeshAgent agent;
    public Vector3 walkPointOrigin;
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (walkPointSet == false)
        {
            //Debug.Log("31");
            float randomZPos = Random.Range(-walkPointRange, walkPointRange);
            float randomXPos = Random.Range(-walkPointRange, walkPointRange);

            walkPointOrigin = new Vector3(agent.transform.position.x + randomXPos, 5, agent.transform.position.z + randomZPos);


            NavMeshHit hit;
            NavMesh.SamplePosition(walkPointOrigin, out hit, 1.0f, NavMesh.AllAreas);
            
                //Debug.Log("31 v1");
                walkPoint = hit.position;
                walkPointSet = true;
            

        }

        if (walkPointSet == true)
        {
            agent.SetDestination(walkPoint);
            if (agent.remainingDistance<=agent.stoppingDistance)
            {
                walkPointSet = false;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
