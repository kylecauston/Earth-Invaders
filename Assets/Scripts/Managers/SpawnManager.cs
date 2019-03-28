using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    public Spawnable[] spawnables;

    private int currentlySelected = -1;
    private SpawnPreview preview;

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
            Destroy(preview.gameObject);
            preview = null;
            return;
        }

        if (currentlySelected == i)
            return;

        currentlySelected = i;
        // when we select a new unit to spawn, create a preview based on selected unit.
        preview = (SpawnPreview)Instantiate(spawnables[currentlySelected].preview);

        // unselect unit if you're in spawn mode
        TheGameManager.GameManager.instance.SelectEntity(null);
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
