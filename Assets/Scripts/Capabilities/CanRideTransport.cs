using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRideTransport : MonoBehaviour
{
    public static void BoardTransport(Component e, Component transport)
    {
        Debug.Log(e.name + " is boarding " + transport.name);
        Transport t = transport.gameObject.GetComponent<Transport>();

        if(Vector3.Distance(e.gameObject.transform.position, t.gameObject.transform.position) < t.boardingRadius)
        {
            t.Board(e.gameObject);
        }
    }
}
