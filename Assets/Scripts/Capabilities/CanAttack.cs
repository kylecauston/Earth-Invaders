﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CanAttack : MonoBehaviour
{
    public static void Attack(Component e, Component targ)
    {
        CanAttack ca = e.gameObject.GetComponent<CanAttack>();
        ca.SetTarget(targ.gameObject.GetComponent<Entity>());
    }

    public Entity target;

    private Entity entityComponent;

    private Weapon weapon;
    private CanMove moveComponent;
    private WaitForSeconds delay;

    private int targetMask; // ignore allies in raycasts
    private float lastFireRate = -1;
    private float secondsBetweenDistanceChecks = 0.5f;
    private float secondsSinceCheck = 0;

    // Cached objects
    private Ray ray;
    private RaycastHit[] raycastHits;
    private NavMeshPath path;
    private Vector3 location;

    // TODO: make this use IEnumerator instead
    // Also add raycast check, which also covers weapon range. Or should this be on Weapon?
    // Also make array of weapons.

    public void Start()
    {
        entityComponent = this.gameObject.GetComponent<Entity>();
        // ignore allied units when calc LoS
        targetMask = ~(1 << (entityComponent.alignment == Globals.Alignment.Earth ? LayerMask.NameToLayer("Earth Units") : LayerMask.NameToLayer("Space Units")));

        weapon = GetComponentInChildren<Weapon>();
        moveComponent = GetComponent<CanMove>();
        delay = new WaitForSeconds(1.0f / weapon.GetFirerate());
        lastFireRate = weapon.GetFirerate();

        ray = new Ray();
        raycastHits = new RaycastHit[10];
        path = new NavMeshPath();
    }

    public void Update()
    {
        secondsSinceCheck += Time.deltaTime;

        // do we need to check if we can still hit?
        if (secondsSinceCheck >= secondsBetweenDistanceChecks)
        {
            secondsSinceCheck = 0;

            // if we can't hit the target from where we are
            if(!CanHitTarget() && target)
            {
                // if we don't have the ability to move
                if (!moveComponent)
                {
                    SetTarget(null);
                }
                else
                {
                    // otherwise, move to somewhere we can attack from
                    FixLineOfSight();
                }
            }
        }
    }

    public void SetTarget(Entity e)
    {
        // Don't do anything when we target the same entity.
        if (e == target)
            return;

        StopAllCoroutines();
        target = e;
        if(target)
        {
            StartCoroutine(Attack());
        }
    }

    public Entity GetTarget()
    {
        return target;
    }

    private bool CanHitTarget()
    {
        ray.origin = this.gameObject.transform.position;
        if (target)
        {
            ray.direction = target.transform.position - this.gameObject.transform.position;
        }
        else
        {
            return false;
        }

        int numHit = Physics.RaycastNonAlloc(ray, raycastHits, weapon.GetMaxRange(), targetMask, QueryTriggerInteraction.Ignore);
        if(numHit == 10)
        {
            Debug.LogError("Raycast has reached capacity. This may cause problems -- consider increasing cache size.");
        }
        if (numHit > 0)
        {
            int closestIndex = 0;
            for(int i=0; i<numHit; i++)
            {
                if(raycastHits[i].distance < raycastHits[closestIndex].distance)
                {
                    closestIndex = i;
                }
            }
            if (target && raycastHits[closestIndex].collider.transform.IsChildOf(target.transform))
                return true;
        }

        return false;
    }

    private void FixLineOfSight()
    {
        if (!moveComponent)
            return;

        // find the quickest path to the target
        NavMesh.CalculatePath(this.transform.position, target.transform.position, NavMesh.AllAreas, path);
        // step through the corners to find the first corner that has line of sight to the target
        Vector3[] corners = path.corners;

        // if a corner has LoS, 
        //   ensure we're close enough by constraining position
        int bestPlace = -1;
        for (int i=0; i<corners.Length; i++)
        {
            ray.origin = corners[i];
            ray.direction = target.transform.position - corners[i];
            // cast to the target from the corner
            if (Physics.RaycastNonAlloc(ray, raycastHits, weapon.GetMaxRange(), targetMask, QueryTriggerInteraction.Ignore) > 0)
            {
                // if we hit the target from this corner, we're done
                if (target && raycastHits[0].collider.transform.IsChildOf(target.transform))
                {
                    bestPlace = i;
                    break;
                }
            }
        }
        
        // if we didn't find a close enough corner, we'll find a 
        //   place along the line between last corner and target.
        if (bestPlace == -1)
        {
            if (corners.Length >= 2)
            {
                location = corners[corners.Length - 2] - target.transform.position;
                location = target.transform.position + location.normalized * weapon.GetMaxRange() * 0.8f;
            }
        }
        else
        {
            location = corners[bestPlace];
        }

        // for now, just move to it.
        // for future, we should ensure it's a good range
        moveComponent.MoveTo(location);
    }

    private void Fire()
    {
        if (target && target.visible)
        {
            weapon.Shoot();
            target.TakeDamage(weapon.GetDamage(), entityComponent);
        }
        else
        {
            SetTarget(null);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (CanHitTarget())
                Fire();

            if (target == null)
                StopAllCoroutines();

            if (lastFireRate != weapon.GetFirerate())
                delay = new WaitForSeconds(1.0f / weapon.GetFirerate());

            yield return delay;
        }
    }

    public void OnDrawGizmos()
    {
        ray.origin = this.gameObject.transform.position;
        if (target)
        {
            ray.direction = target.transform.position - this.gameObject.transform.position;
        }
        else
        {
            ray.direction = this.gameObject.transform.forward;
        }
        Gizmos.color = CanHitTarget() ? Color.green : Color.red;
        if(weapon)
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * weapon.GetMaxRange());
    }
}
