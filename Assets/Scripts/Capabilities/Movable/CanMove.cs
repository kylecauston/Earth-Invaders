using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CanMove : MonoBehaviour
{
    public abstract void MoveTo(Vector3 v);
    public abstract bool Arrived();
}
