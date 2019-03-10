using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum SelectionShape { Square, Circle };

    public int maxHealth;
    public int health;
    public Globals.Alignment alignment;
    public Globals.Classification classification;
    public SelectionShape ringShape = SelectionShape.Circle;
    public float ringRadius = 5;
    public float bandWidth = 5;

    public bool visible = true;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }
   
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDeath();
    }

    public void GetHealed(int gain)
    {
        int last_hp = health;
        health = Mathf.Min(health + gain, maxHealth);
        Debug.Log(name + " was healed for " + (health - last_hp));
    }

    public void OnDeath()
    {
        //Destroy(gameObject);
        // TODO: Stop all coroutines
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
