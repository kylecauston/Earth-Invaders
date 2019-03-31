using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttack : MonoBehaviour
{
    public Entity target;
    public float time;

    private Weapon weapon;

    // TODO: make this use IEnumerator instead
    // Also add raycast check, which also covers weapon range. Or should this be on Weapon?
    // Also make array of weapons.

    public void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    public static void Attack(Component e, Component targ)
    {
        e.gameObject.GetComponent<CanAttack>().target = targ.gameObject.GetComponent<Entity>();
    }

    public void Update()
    {
        time += Time.deltaTime;
        
        if (time >= 1 && target)
        {
            time = 0.0f;
            if (target.visible)
            {
                weapon.Shoot();
                target.TakeDamage(1);
            }
            else
            {
                target = null;
            }
        }
    }
}
