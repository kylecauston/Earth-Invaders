using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Building : MonoBehaviour
{
    public GameObject[] entrances;
    public static int entranceThreshold = 3;

    private Entity self = null;
    private Storage storage;

    public void Start()
    {
        self = GetComponent<Entity>();
        storage = GetComponent<Storage>();
    }

    public bool Enter(GameObject go)
    {
        self.alignment = go.GetComponent<Entity>().alignment;
        return storage.Store(go);
    }

    public GameObject RemoveEntity(int i)
    {
        GameObject go = storage.Remove(i);
        // if the building is now empty
        if (storage.GetNumStored() == 0)
        {
            self.alignment = Globals.Alignment.Neutral;
            if (TheGameManager.GameManager.instance.GetSelectedEntity().gameObject == this.gameObject)
            {
                TheGameManager.GameManager.instance.SelectEntity(go.GetComponent<Entity>());
            }
        }
        // we can actually just return the gameobject here, the building is never going to move (i hope)

        return go;
    }
}
