﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRideTransport : MonoBehaviour
{
    public static void BoardTransport(Component e, Component transport)
    {
        Debug.Log(e.name + " is boarding " + transport.name);
    }
}