using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Transport : MonoBehaviour
{
    private Storage storage;
    public int boardingRadius = 8;

    public void Start()
    {
        storage = GetComponent<Storage>();
    }

    public bool Board(GameObject go)
    {
        return storage.Store(go);
    }

    public GameObject RemoveEntity(int i)
    {
        // we need to move the entity to somewhere within the range of the transport
        GameObject go = storage.Remove(i);
        if (!go)
            return null; 

        Vector3 offset = Random.insideUnitCircle * boardingRadius;
        offset.z = offset.y;
        offset.y = 0;
        offset *= (Random.Range(0, 1) < 0.5 ? -1 : 1);
        Debug.Log(offset);
        go.transform.SetPositionAndRotation(this.transform.position + offset, go.transform.rotation);
        return go;
    }
}
