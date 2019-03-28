using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSpawning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        Material mat = r.material;
        Bounds b = r.bounds;
        Vector3 offset = new Vector3(0, b.extents.y, 0);
        mat.SetVector("_Bottom", b.center - offset);
    }
}
