using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFlock : FlockMovement
{
    private enum Mode { Wander, Seek };
    private Mode mode = Mode.Wander;

    public void FixedUpdate()
    {
        Vector3 desired = cached;
        if (mode == Mode.Seek)
        {
            desired = Arrive(target);
            if(Vector3.Distance(target, rb.position) < 2)
            {
                mode = Mode.Wander;
                target = NULL;
                Debug.Log("Wander time!");
            }
        }
        else if (mode == Mode.Wander)
        {
            desired = Wander();
        }

        SetDesiredSpeed(desired);
    }

    public override void MoveTo(Vector3 v)
    {
        target = v;
        mode = Mode.Seek;
    }

    public override bool Arrived()
    {
        throw new System.NotImplementedException();
    }
}
