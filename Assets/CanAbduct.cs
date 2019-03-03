using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAbduct : MonoBehaviour
{
    public static void Abduct(Component primary, Component victim)
    {
        Debug.Log(primary.name + " is abducting " + victim.name);
    }
}
