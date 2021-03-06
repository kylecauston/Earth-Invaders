﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPreview : MonoBehaviour
{
    public SpawningMechanics mechanics;
    public Material validMaterial;
    public Material invalidMaterial;

    private List<string> collisionTags;
    private enum Mode { Valid, Invalid }
    private Mode currentMode = Mode.Invalid;
    private Renderer[] renderers;

    public void Start()
    {
        collisionTags = new List<string>();
        renderers = GetComponentsInChildren<Renderer>();
        InvalidMode();
    }

    public void Update()
    {
        bool canSpawn = mechanics.CanSpawnHere(this);
        if (canSpawn && currentMode == Mode.Invalid)
        {
            ValidMode();
        }
        else if (!canSpawn && currentMode == Mode.Valid)
        {
            InvalidMode();
        }
    }

    private void InvalidMode()
    {
        currentMode = Mode.Invalid;
        for(int i=0; i<renderers.Length; i++)
        {
            renderers[i].material = invalidMaterial;
        }
    }

    private void ValidMode()
    {
        currentMode = Mode.Valid;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = validMaterial;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        collisionTags.Add(collision.gameObject.tag);
    }

    private void OnTriggerExit(Collider collision)
    {
        collisionTags.Remove(collision.gameObject.tag);
    }

    public bool CollidesWithTag(string tag)
    {
        return collisionTags.Contains(tag);
    }
    
    // Returns if there's any collisions that aren't in the ignored list.
    public bool NoCollisions(List<string> ignored)
    {
        for(int i=0; i<collisionTags.Count; i++)
        {
            // if this collider isn't in the ignored list
            if(!ignored.Contains(collisionTags[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool CanSpawn()
    {
        return currentMode == Mode.Valid;
    }
}
