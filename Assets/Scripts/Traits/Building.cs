using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Building : MonoBehaviour
{
    private Storage storage;
    public GameObject[] entrances;
    public static int entranceThreshold = 3;

    public void Start()
    {
        storage = GetComponent<Storage>();
    }

    public bool Enter(GameObject go)
    {
        return storage.Store(go);
    }

    public GameObject RemoveEntity(int i)
    {
        // we can actually just return the gameobject here, the building is never going to move (i hope)
        return storage.Remove(i);
    }
}
