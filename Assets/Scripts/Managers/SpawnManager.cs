using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    public Spawnable[] spawnables;

    private int currentlySelected = -1;
    private GameObject preview;

    private Vector3 cached = new Vector3();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetSelected()
    {
        return currentlySelected;
    }

    public void SetSelected(int i)
    {
        if(i < 0 || i > spawnables.Length)
        {
            currentlySelected = -1;
            preview = null;
            return;
        }

        currentlySelected = i;
        // when we select a new unit to spawn, create a preview based on selected unit.
        preview = (GameObject)Instantiate(spawnables[currentlySelected].preview);
    }

    private void Update()
    {
        if (currentlySelected < 0)
            return;

        // if we have something we want to spawn, show preview.
        if(InputManager.instance.GetMouseTerrainCoords(out cached))
        {
            preview.transform.SetPositionAndRotation(cached, preview.transform.rotation);
        }
    }

    public void Spawn(Vector3 pos)
    {
        if (currentlySelected < 0)
            return;
        
        Spawnable sp = Instantiate<Spawnable>(spawnables[currentlySelected], pos, Quaternion.identity);
        sp.Spawn(pos);
    }
}
