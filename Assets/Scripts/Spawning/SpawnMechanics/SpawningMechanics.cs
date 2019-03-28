using System.Collections.Generic;
using UnityEngine;

public class SpawningMechanics : ScriptableObject
{
    protected List<string> ignoredCollisions = new List<string>();

    public void OnEnable()
    {
        if(!ignoredCollisions.Contains("Terrain"))
            ignoredCollisions.Add("Terrain");
    }

    public virtual bool CanSpawnHere(SpawnPreview preview)
    {
        return preview.NoCollisions(ignoredCollisions);
    }
}
