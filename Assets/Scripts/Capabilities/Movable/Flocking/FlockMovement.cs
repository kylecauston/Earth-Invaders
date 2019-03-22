using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class FlockMovement : CanMove
{
    public int maxSpeed = 10;
    public int slowRadius = 10;
    public int wanderRadius = 10;

    protected static Vector3 NULL = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

    protected Vector3 target;
    protected Vector3 cached = new Vector3(0, 0, 0);

    protected Rigidbody rb;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDesiredSpeed(Vector3 desired)
    {
        Vector3 force = desired - rb.velocity;
        rb.AddForce(force);

        if (rb.velocity.sqrMagnitude != 0)
        {
            Quaternion orient = new Quaternion();
            orient.SetLookRotation(rb.velocity, Vector3.up);
            orient *= Quaternion.AngleAxis(180, Vector3.up);
            this.transform.rotation = orient;
        }
    }

    protected Vector3 Arrive(Vector3 t)
    {
        Vector3 toTarg = t - rb.transform.position;
        float sqrDist = toTarg.sqrMagnitude;
        Vector3 seek = Seek(t);

        // If we're in the slow radius, start to slow down.
        if (sqrDist < Mathf.Pow(slowRadius, 2))
        {
            seek *= sqrDist / Mathf.Pow(slowRadius, 2);
        }

        return seek;
    }

    protected Vector3 Seek(Vector3 t)
    {
        Vector3 toTarg = t - rb.transform.position;
        float sqrDist = toTarg.sqrMagnitude;
        toTarg.Normalize();
        toTarg *= maxSpeed;
        return toTarg;
    }

    protected Vector3 Wander()
    {
        if (Random.Range(0.0f, 1.0f) < 0.1 || target.Equals(NULL))
        {
            Vector3 unitPoint = Random.insideUnitCircle * wanderRadius;
            unitPoint.Set(unitPoint.x, 0, unitPoint.y);
            Vector3 direction = rb.velocity.normalized * 10;
            target = direction + unitPoint + rb.position;
            if (target.x > 25 || target.z > 25 || target.x < -25 || target.z < -25)
            {
                target.Set(0, target.y, 0);
            }
        }

        Vector3 desired = Seek(target);
        desired.Normalize();
        desired *= maxSpeed; // 2.0f;
        return desired;
    }
}
