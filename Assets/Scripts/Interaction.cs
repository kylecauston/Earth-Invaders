using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interaction : IComparable<Interaction>
{
    public enum Priority { Lowest, Low, Medium, High, Highest }

    public Interaction(string n, Action<Component, Component> a, AgentAI.Task t)
    {
        this.name = n;
        this.action = a;
        this.priority = Priority.Medium;
        this.task = t;
    }

    public Interaction(string n, Action<Component, Component> a, Priority p, AgentAI.Task t): this(n, a, t)
    {
        this.priority = p;
    }

    private string name;
    private Action<Component, Component> action;
    private Priority priority;
    private AgentAI.Task task;

    public string GetName() { return name; }
    public Action<Component, Component> GetAction() { return action; }
    public Priority GetPriority() { return priority; }
    public AgentAI.Task GetTask() { return task; }

    public int CompareTo(Interaction other)
    {
        return other.GetPriority() - this.GetPriority();
    }

    public void Trigger(Component initiator, Component target)
    {
        GetAction().Invoke(initiator, target);
    }
}
