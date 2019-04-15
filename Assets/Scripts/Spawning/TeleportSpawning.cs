using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSpawning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Renderer[] r = GetComponents<Renderer>();
        if (r.Length == 0)
            r = GetComponentsInChildren<Renderer>();

        float lowest = Mathf.Infinity;
        for(int i=0; i<r.Length; i++)
        {
            Bounds b = r[i].bounds;
            if(b.extents.y < lowest)
            {
                lowest = b.extents.y;
            }
        }
        
        Vector3 offset = new Vector3(0, lowest, 0);

        for (int i = 0; i < r.Length; i++)
        {
            Material mat = r[i].material;
            Bounds b = r[i].bounds;
            mat.SetVector("_Bottom", b.center - offset);
        }
    }
}
