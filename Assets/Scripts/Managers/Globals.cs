using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals instance = null;

    public enum Alignment { Earth, Space, Neutral };
    public enum Classification { None, Infantry, Heavy, Aerial };
    public enum Allegiance { Allied, Enemy, Neutral };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Allegiance GetAllegiance(Entity e1, Entity e2)
    {
        if (e1.alignment == e2.alignment)
            return Allegiance.Allied;
        else if (e1.alignment == Alignment.Neutral || e2.alignment == Alignment.Neutral)
            return Allegiance.Neutral;
        else
            return Allegiance.Enemy;
    }
}
