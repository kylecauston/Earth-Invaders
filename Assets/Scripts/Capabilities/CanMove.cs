using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CanMove : MonoBehaviour
{
    private Camera camera;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main;
    }

    public void MoveTo(Vector3 v)
    {
        agent.SetDestination(v);
    }
}
