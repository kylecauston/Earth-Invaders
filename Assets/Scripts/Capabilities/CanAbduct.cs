using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class CanAbduct : MonoBehaviour
{
    public float pullStrength = 0.5f;
    public int abductRadius = 8;

    private int capturedRadius = 3;   // The distance a victim has to be before being stored
    private Storage storage;

    // cached values
    private Vector3 toShip;
    private WaitForSeconds delay = new WaitForSeconds(0.01f);

    public void Start()
    {
        storage = GetComponent<Storage>();
    }

    public static void Abduct(Component primary, Component victim)
    {
        CanAbduct ca = primary.gameObject.GetComponent<CanAbduct>();
        ca.StartCoroutine(ca.TractorBeam(victim.gameObject));
        //victim.gameObject.SetActive(false);
        // TODO: Active ragdoll
        // Need to deactivate victim.NavMeshAgent but have a way to re-enable it if dropped from tractor beam????
        // Or just find what is causing the agent to break.

        // TODO: Consider OnDeath calling OnDeath methods for each component, letting them clean up their own stuff.
        //      This would allow CanAbduct to return it's victims to regular states.

        // We could call StartCapture on victim, and call StopCapture if we let them go.
        // These methods could handle enabling and disabling their own navmesh. 
        // However, this still requires the OnDeath change.
    }

    // TODO: This should really be it's own component, since not everything that captures would use a tractor beam
    IEnumerator TractorBeam(GameObject targ)
    {
        // Pull the victim until they're close enough to capture
        while (Vector3.Distance(targ.transform.position, this.transform.position) > capturedRadius)
        {
            Vector3 myPos = this.gameObject.transform.position;
            Vector3 targPos = targ.transform.position;
            toShip.Set(myPos.x - targPos.x, myPos.y - targPos.y, myPos.z - targPos.z);
            toShip.Normalize();
            // TODO: Consider using Animation for this? either just using it for making the movement look
            //      good or literally using an animation.
            // There's no reason this needs to be physically moving the entity.
            float v = 0.2f + (Mathf.Pow(1 - (Mathf.Abs(myPos.y - targPos.y) / myPos.y), 2));
            Debug.Log(v);
            toShip *= pullStrength * v;
            // pull target slightly closer
            targ.transform.Translate(toShip);
            yield return delay;
        }

        storage.Store(targ);
        yield break;
    }
}
