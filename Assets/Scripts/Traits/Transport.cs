using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Transport : MonoBehaviour
{
    public Storage storage;
    public int boardingRadius = 8;

    public bool Board(GameObject go)
    {
        return storage.Store(go);
    }
}
