using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterBuilding : MonoBehaviour
{
    public static void EnterBuilding(Component e, Component building)
    {
        Debug.Log(e.name + " is entering " + building.name);
        building.gameObject.GetComponent<Building>().Enter(e.gameObject);
    }
}
