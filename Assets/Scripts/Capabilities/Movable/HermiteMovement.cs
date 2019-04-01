using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteMovement : CanMove
{
    private Vector3 p0, p1, m0, m1;

    private Vector3 HermiteSpline(float t)
    {
        float t2 = Mathf.Pow(t, 2);
        float t3 = Mathf.Pow(t, 3);
        return (2*t3 - 3*t2 + 1) * p0 + (t3 - 2*t2 + t) * m0 + (-2*t3 + 3*t2) * p1 + (t3 - t2) * m1;
    }

    // formula from this great github
    // https://gist.github.com/gre/1650294
    private float QuadEaseInOut(float t)
    {
        return (t < 0.5) ? (2 * Mathf.Pow(t, 2)) : (-1 + (4 - 2 * t) * t);
    }

    public override void MoveTo(Vector3 v)
    {
        StopAllCoroutines();
        p0 = this.gameObject.transform.position;
        m0 = this.gameObject.transform.rotation * Vector3.forward * 10 + p0;
        p1 = new Vector3(v.x, p0.y, v.z);
        m1 = Vector3.Normalize(p1 - p0)*10 + p1;

        StartCoroutine(Move());
    }

    // TODO: this is still very jittery
    private IEnumerator Move()
    {
        for (float s = 0.0f; s < 1.0f; s += 0.01f)
        {
            float sp = QuadEaseInOut(s);
            this.transform.position = HermiteSpline(sp);
            Vector3 dir = HermiteSpline(sp + 0.01f) - HermiteSpline(sp); // get the direction to face
            dir.Normalize();

            // face that direction
            Quaternion orient = new Quaternion();
            orient.SetLookRotation(dir, Vector3.up);
            this.transform.rotation = orient;
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }

    public override bool Arrived()
    {
        // not implmented since we don't use it right now
        throw new System.NotImplementedException();
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
}
