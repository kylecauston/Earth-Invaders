using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interaction : IComparable<Interaction>
{
    public enum Priority { Lowest, Low, Medium, High, Highest }

    public Interaction(string n, Action<Component, Component> a)
    {
        this.name = n;
        this.action = a;
        this.priority = Priority.Medium;
    }

    public Interaction(string n, Action<Component, Component> a, Priority p): this(n, a)
    {
        this.priority = p;
    }

    private string name;
    private Action<Component, Component> action;
    private Priority priority;

    public string GetName() { return name; }
    public Action<Component, Component> GetAction() { return action; }
    public Priority GetPriority() { return priority; }

    public int CompareTo(Interaction other)
    {
        return other.GetPriority() - this.GetPriority();
    }
}
