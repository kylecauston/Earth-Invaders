using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterBuilding : MonoBehaviour
{
    public static void EnterBuilding(Component e, Component building)
    {
        Building b = building.gameObject.GetComponent<Building>();
        GameObject[] entrances = b.entrances;
        for(int i=0; i<entrances.Length; i++)
        {
            if(Vector3.Distance(e.gameObject.transform.position, entrances[i].transform.position) < Building.entranceThreshold)
            {
                b.Enter(e.gameObject);
                Debug.Log(e.name + " is entering " + building.name);
                // check if this is our selected unit, if it is, select the building now.
                if(TheGameManager.GameManager.instance.GetSelectedEntity().gameObject == e.gameObject)
                {
                    TheGameManager.GameManager.instance.SelectEntity(b.GetComponent<Entity>());
                }
                break;
            }
        }
       
    }
}
