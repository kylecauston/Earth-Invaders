using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerFlock : FlockMovement
{
    public int neighborRadius = 20;
    public float alignmentStrength = 1.0f;
    public float cohesionStrength = 1.0f;
    public float separationStrength = 1.0f;
    public float followDistance = 1;

    public Rigidbody leader;

    private Vector3 total;
    private List<Rigidbody> neighbors; // cached to prevent constantly making new lists
    private Collider[] results = new Collider[10]; // Need to monitor this to ensure it's an okay amount.

    public override void Awake()
    {
        base.Awake();
        
        /*// followers can't be selected.
        Collider selector = GetComponent<Collider>();
        if(selector)
        {
            selector.enabled = false;
        }*/

        neighbors = new List<Rigidbody>();
        total = new Vector3(0, 0, 0);
    }

    // Move in the average direction of the crowd.
    private Vector3 Alignment()
    {
        cached.Set(0, 0, 0);
        for(int i=0; i<neighbors.Count; i++)
        {
            cached += neighbors[i].velocity.normalized;
        }
        if (neighbors.Count > 0)
            cached /= neighbors.Count;

        return cached;
    }

    // Move towards the center of the crowd.
    private Vector3 Cohesion()
    {
        cached.Set(0, 0, 0);

        for (int i = 0; i < neighbors.Count; i++)
        {
            cached += neighbors[i].position;
        }
        if (neighbors.Count > 0)
        {
            cached /= neighbors.Count;
            cached -= rb.position;
        }
        return cached;
    }

    // Don't get too close to anyone.
    private Vector3 Separation()
    {
        cached.Set(0, 0, 0);
        for (int i = 0; i < neighbors.Count; i++)
        {
            cached += rb.position - neighbors[i].position;
        }
        if(neighbors.Count > 0)
            cached /= neighbors.Count;

        return cached;
    }

    private Vector3 FollowTheLeader()
    {
        Vector3 behindLeader = leader.position - leader.velocity.normalized * followDistance;
        //Debug.Log(name + " moving to " + behindLeader);
        Vector3 a = Arrive(behindLeader);
        //Debug.Log("Force:" + a);
        return a;
    }

    private void UpdateNeighborList()
    {
        neighbors.Clear();
        int numFound = Physics.OverlapSphereNonAlloc(this.gameObject.transform.position, neighborRadius, results);
        GameObject go = null;
        for (int i=0; i<numFound; i++)
        {
            go = results[i].gameObject;
            if (go != this.gameObject && go != leader.gameObject && go.GetComponent<FlockMovement>() != null)
            {
                neighbors.Add(go.GetComponent<Rigidbody>());
            }
        }
    }

    public void FixedUpdate()
    {
        if(Random.Range(0.0f, 1.0f) > 0.5f)
        {
            UpdateNeighborList();
        }

        total.Set(0, 0, 0);
        total += alignmentStrength * Alignment();
        total += cohesionStrength * Cohesion();
        total += separationStrength * Separation();
        total += FollowTheLeader();
        total.Normalize();
        total *= maxSpeed;
        //Debug.Log("total: " + total);
        SetDesiredSpeed(total);
    }

    public override void MoveTo(Vector3 v)
    {
        // do nothing, flocks can't be assigned movement
        // or do we set this to be the seek??
    }

    public override bool Arrived()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
}
