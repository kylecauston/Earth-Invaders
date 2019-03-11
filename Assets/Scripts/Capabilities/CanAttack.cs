using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttack : MonoBehaviour
{
    public Entity target;

    public float time;

    public static void Attack(Component e, Component targ)
    {
        Debug.Log(e.name + " attacks " + targ.name);
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
                target.TakeDamage(1);
            }
            else
            {
                target = null;
            }
        }
    }
}
