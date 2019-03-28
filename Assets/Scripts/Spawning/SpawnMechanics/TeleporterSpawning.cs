using UnityEngine;

public class TeleporterSpawning : SpawningMechanics
{
    public void OnEnable()
    {
        base.OnEnable();
        if(!ignoredCollisions.Contains("Teleporter"))
            ignoredCollisions.Add("Teleporter");
    }

    public override bool CanSpawnHere(SpawnPreview preview)
    {
        return base.CanSpawnHere(preview) && preview.CollidesWithTag("Teleporter");
    }
}
