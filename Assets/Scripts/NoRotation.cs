using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotation : MonoBehaviour
{
    private Quaternion rot;
    void Awake()
    {
        rot = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.rotation = rot;
    }
}
