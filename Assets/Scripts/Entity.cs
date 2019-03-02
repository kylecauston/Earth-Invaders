using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public Globals.Alignment alignment;
    public Globals.Classification classification;

    public static void Attack(Component initiator, Component target)
    {
        Debug.Log(initiator.name + " attacks " + target.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAllied(Entity e)
    {
        return e.alignment == this.alignment;
    }

    public bool IsEnemy(Entity e)
    {
        return !(IsAllied(e));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void OnMouseEnter()
    {
        InputManager.instance.HoverObject(this);
    }

    public void OnMouseExit()
    {
        InputManager.instance.HoverObject(null);
    }

    public void OnMouseDown()
    {
        InputManager.instance.ClickObject(this);
    }
}
