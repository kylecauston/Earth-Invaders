using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyNewWeaponData", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int minRange;
    public int maxRange;
    public int damagePerShot;
    public int firerate;

}
