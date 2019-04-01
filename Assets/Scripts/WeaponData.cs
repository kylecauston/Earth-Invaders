using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyNewWeaponData", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int minRange;
    public int maxRange;
    public int damagePerShot;
    public float firerate;

    public void OnValidate()
    {
        firerate = Mathf.Max(0.1f, firerate);
        damagePerShot = Mathf.Max(0, damagePerShot);
        minRange = Mathf.Max(0, minRange);
        maxRange = Mathf.Max(0, maxRange);
    }
}
