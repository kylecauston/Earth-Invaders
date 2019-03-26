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

}
