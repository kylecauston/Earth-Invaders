using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAura : Aura
{
    public int healAmount = 1;

    public override void Effect(GameObject go)
    {
        Entity e = go.GetComponent<Entity>();
        if (!e || e.classification == Globals.Classification.Building) return;

        e.GetHealed(healAmount);
    }
}
