using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private List<GameObject> stored;
    public int capacity = -1;

    public void Start()
    {
        stored = new List<GameObject>();
    }

    public List<GameObject> GetStored()
    {
        return stored;
    }

    public bool Store(GameObject go)
    {
        if (stored.Count >= capacity && capacity != -1) return false;

        stored.Add(go);
        go.GetComponent<Entity>().visible = false;
        go.SetActive(false);

        return true;
    }

    public void Remove(GameObject go)
    {
        Remove(stored.IndexOf(go));
    }

    public void Remove(int i)
    {
        GameObject go = stored[i];
        stored.RemoveAt(i);
        go.GetComponent<Entity>().visible = true;
        go.SetActive(true);
    }

    public List<GameObject> EmptyOut()
    {
        List<GameObject> list = new List<GameObject>();

        for(int i=0; i<stored.Count; i++)
        {
            list.Add(stored[i]);
        }
        
        stored.Clear();
        
        return list;
    }
}
