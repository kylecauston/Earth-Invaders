using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttack : MonoBehaviour
{
    public static void Attack(Component e, Component targ)
    {
        Debug.Log(e.name + " attacks " + targ.name);
    }
}
