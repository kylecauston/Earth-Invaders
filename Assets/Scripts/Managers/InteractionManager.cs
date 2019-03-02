using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance = null;
    public string NO_ACTION = "NO_ACTION";

    private Dictionary<Tuple<Type, Type, Globals.Allegiance>, Interaction> interactions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        interactions = new Dictionary<Tuple<Type, Type, Globals.Allegiance>, Interaction>();

        SetInteraction(typeof(Entity), typeof(Entity), Globals.Allegiance.Enemy, "Attack", Entity.Attack);
        SetInteraction(typeof(Entity), typeof(Entity), Globals.Allegiance.Neutral, "Attack", Entity.Attack, Interaction.Priority.Lowest);

        SetInteraction(typeof(CanEnterBuilding), typeof(Building), Globals.Allegiance.Neutral, "Enter Building", CanEnterBuilding.EnterBuilding);
        SetInteraction(typeof(CanEnterBuilding), typeof(Building), Globals.Allegiance.Enemy, "Storm Building", CanEnterBuilding.EnterBuilding, Interaction.Priority.Low);
        SetInteraction(typeof(CanEnterBuilding), typeof(Building), Globals.Allegiance.Allied, "Enter Building", CanEnterBuilding.EnterBuilding);

        SetInteraction(typeof(CanRideTransport), typeof(Transport), Globals.Allegiance.Allied, "Board Transport", CanRideTransport.BoardTransport);
    }

    public void SetInteraction(Type t1, Type t2, Globals.Allegiance targ, string name, Action<Component, Component> fn, Interaction.Priority p = Interaction.Priority.Medium)
    {
        interactions.Add(new Tuple<Type, Type, Globals.Allegiance>(t1, t2, targ), 
            new Interaction(name, fn, p));
    }

    // Return the Action that is called when c1 interacts with c2.
    public Interaction GetInteraction(Component c1, Component c2, Globals.Allegiance a)
    {
        // TODO: cache tuple
        Tuple<Type, Type, Globals.Allegiance> t = new Tuple<Type, Type, Globals.Allegiance>(c1.GetType(), c2.GetType(), a);
        if (interactions.ContainsKey(t))
        {
            return interactions[t];
        }
        else
        {
            return null;
        }
    }

    // Return a list of all interactions caused by initator interacting with target.
    public List<Interaction> GetInteractions(Entity initiator, Entity target)
    {
        // TODO: cache list
        List<Interaction> list = new List<Interaction>();
        Component[] initComponents = initiator.gameObject.GetComponents<Component>();
        Component[] targComponents = target.gameObject.GetComponents<Component>();

        Interaction a;
        Globals.Allegiance allegiance = Globals.instance.GetAllegiance(initiator, target);
        foreach(Component c1 in initComponents)
        {
            foreach(Component c2 in targComponents)
            {
                a = GetInteraction(c1, c2, allegiance);
                if(a != null)
                {
                    list.Add(a);
                }
            }
        }

        return list;
    }

    // Perform the ith interaction between initiator and target.
    public bool Interact(Entity initiator, Entity target, int i)
    {
        // TODO: Cache interaction list
        List<Interaction> interactions = GetInteractions(initiator, target);
        if (interactions.Count == 0)
            return false;
        interactions.Sort();
        for(int j=0; j<interactions.Count; j++)
            Debug.Log(initiator.name + " can " + interactions[j].GetName() + " [" + target.name + "] with prio=" + interactions[j].GetPriority());

        i = Mathf.Clamp(i, 0, interactions.Count);
        interactions[i].GetAction().Invoke(initiator, target);
        return true;
    }
}
