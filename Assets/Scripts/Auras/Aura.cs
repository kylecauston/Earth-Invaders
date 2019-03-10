using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aura : MonoBehaviour
{
    public abstract void Effect(GameObject go);

    public float delay = 1.0f;
    public float radius = 10;
    public Globals.Allegiance affects;

    private WaitForSeconds delayTimer;
    private Collider[] results = new Collider[30]; // Need to monitor this to ensure it's an okay amount.
    private Entity self = null;

    public void Start()
    {
        delayTimer = new WaitForSeconds(delay);
        self = this.gameObject.GetComponent<Entity>();
        StartCoroutine(Activate());
    }

    // TODO: Should this use physics.sphere check, or a trigger?
    /*
     * Finds all Entities within [radius] and calls Effect with them as param.
     */
    IEnumerator Activate()
    {
        while (true)
        {
            int numFound = Physics.OverlapSphereNonAlloc(this.gameObject.transform.position, radius, results);
            if (numFound >= 30)
                throw new System.Exception("Reached collider capacity in " + gameObject.name);

            for (int i = 0; i < numFound; i++)
            {
                Effect(results[i].gameObject);
            }
            yield return delayTimer;
        }
    }
}
