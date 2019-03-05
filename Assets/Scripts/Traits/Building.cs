﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Building : MonoBehaviour
{
    public Storage storage;

    public bool Enter(GameObject go)
    {
        return storage.Store(go);
    }
}
