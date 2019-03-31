using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : CanMove
{
    private NavMeshAgent agent;
    private Animator anim;
    private int speedID = Animator.StringToHash("Speed");
    private int deathID = Animator.StringToHash("Die");

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim)
        {
            anim.SetFloat(speedID, agent.velocity.magnitude);

            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetTrigger(deathID);
                if (agent.speed != 0)
                {
                    agent.speed = 0;
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                else
                {
                    agent.isStopped = false;
                    agent.ResetPath();
                    agent.speed = 10;
                }
            }
        }
    }

    public override void MoveTo(Vector3 v)
    {
        agent.SetDestination(v);
    }

    public override bool Arrived()
    {
        // after lots of trial and lots of error the solution posed here
        // http://answers.unity.com/answers/746157/view.html
        // seems to be the best solution
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
