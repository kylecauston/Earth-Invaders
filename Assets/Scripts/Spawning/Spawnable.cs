using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Spawnable : MonoBehaviour
{
    public SpawnPreview preview;
    public GameObject unitToSpawn;
    public int cost;
    public string animationName;
    public Sprite uiIcon;
    public bool positionMoving = true;
    
    private Vector3 offset;

    public void Awake()
    {
        offset = this.transform.position;
    }

    public void Spawn(Vector3 pos)
    {
        offset = pos;
    }

    public void LateUpdate()
    {
        // if animation is a moving animation we need to 
        // make it 'local' by keeping the offset
        // this is because unity position animation is global
        if (positionMoving)
        {
            this.transform.localPosition += offset; 
        }
    }

    private void DeployUnit()
    {
        GameObject go = Instantiate<GameObject>(unitToSpawn, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
