using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAI : MonoBehaviour
{
    public enum Task { Idle, EnterBuilding, Attack, BoardTransport, Abduct };

    private CanMove moveComponent;
    private CanAttack attackComponent;
    private CanEnterBuilding enterComponent;
    private CanRideTransport rideComponent;
    private CanAbduct abductComponent;

    private Task currentTask;
    private Component initiator, target;
    private Interaction onArrive;

    private void Start()
    {
        moveComponent = GetComponent<CanMove>();
        attackComponent = GetComponent<CanAttack>();
        enterComponent = GetComponent<CanEnterBuilding>();
        rideComponent = GetComponent<CanRideTransport>();
        abductComponent = GetComponent<CanAbduct>();
    }

    public void AssignTask(Interaction interaction, GameObject go)
    {
        onArrive = interaction;
        Task task = onArrive.GetTask();
        if(attackComponent)
        {
            attackComponent.SetTarget(null);
        }
        if (task == Task.EnterBuilding)
        {
            if (!moveComponent)
                return;

            Building b = go.GetComponent<Building>();
            target = b;
            initiator = enterComponent;

            // find entrances to building
            GameObject[] entrances = b.entrances;
            // if we're close enough to one, enter
            // if not:
            //  compute paths
            //  find closest path
            //  set move destination

            float[] distance = new float[entrances.Length];
            int minIndex = 0;
            for (int i = 0; i < entrances.Length; i++)
            {
                if (Vector3.Distance(entrances[i].transform.position, this.transform.position) < Building.entranceThreshold)
                {
                    ResolveInteraction();
                    return;
                }

                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(this.transform.position, entrances[i].transform.position, NavMesh.AllAreas, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    Vector3[] corners = path.corners;
                    for (int j = 0; j < corners.Length - 1; j++)
                    {
                        distance[i] += Vector3.Distance(corners[j], corners[j + 1]);
                    }
                }
                if (distance[i] <= distance[minIndex])
                    minIndex = i;
            }

            // at this point we haven't hit an entrance we can enter immediately, 
            // so we need to move towards the closest entrance
            moveComponent.MoveTo(entrances[minIndex].transform.position);
        }
        else if (task == Task.Attack)
        {
            // determine if any weapons can reach target
            // if one can, switch to it and start attacking
            // if not, find a suitable location and Move to it
            //      suitable location = location in range and LoS to enemy
            target = go.GetComponent<Entity>();
            initiator = attackComponent;
            ResolveInteraction();
            return;
        }
        else if (task == Task.BoardTransport)
        {
            // determine transport board range
            Transport transport = go.GetComponent<Transport>();
            target = transport;
            initiator = rideComponent;

            // if we're in radius, board transport
            Vector3 toUs = this.transform.position - go.transform.position;

            if (toUs.sqrMagnitude < Mathf.Pow(transport.boardingRadius, 2))
            {
                ResolveInteraction();
                return;
            }

            if (!moveComponent)
                return;
            // if not, find close location and move to it
            toUs = toUs.normalized * transport.boardingRadius / 2;
            moveComponent.MoveTo(toUs + transport.gameObject.transform.position);
        }
        else if (task == Task.Abduct)
        {
            // determine abduct range
            Abductable abductable = go.GetComponent<Abductable>();
            target = abductable;
            initiator = abductComponent;

            // if we're close enough, abduct
            Vector3 toUs = this.transform.position - go.transform.position;

            if (toUs.sqrMagnitude < Mathf.Pow(abductComponent.abductRadius, 2))
            {
                ResolveInteraction();
                return;
            }

            if (!moveComponent)
                return;

            // if not, find close location and move to it
            toUs = toUs.normalized * abductComponent.abductRadius / 2;
            moveComponent.MoveTo(toUs + abductable.gameObject.transform.position);
        }
    }

    public void Update()
    {
       
        // if we have arrived, trigger the desired interaction
        if (onArrive != null && target && initiator && moveComponent && moveComponent.Arrived())
        {
            ResolveInteraction();
        }
    }

    public void HandleDamage(Entity attacker)
    {
        if (attackComponent && attackComponent.target == null && moveComponent && !moveComponent.IsMoving())
            attackComponent.SetTarget(attacker);
    }

    private void ResolveInteraction()
    {
        onArrive.Trigger(initiator, target);
        onArrive = null;
    }
}
