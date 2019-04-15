using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    public Spawnable[] spawnables;
    public ToggleGroup spawnButtons;

    private int currentlySelected = -1;
    public SpawnPreview preview;

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
        if (preview)
        {
            Destroy(preview.gameObject);
        }

        if (i < 0 || i > spawnables.Length)
        {
            currentlySelected = -1;
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

    public void ToggleSpawning(bool spawning)
    {
        if(!spawning)
            SetSelected(-1);
    }

    public void Update()
    {
        if (currentlySelected < 0)
            return;

        if (TheGameManager.GameManager.instance.currency < spawnables[currentlySelected].cost)
            SetSelected(-1);

        // if we have something we want to spawn, show preview.
        if (preview && InputManager.instance.GetMouseTerrainCoords(out cached))
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
