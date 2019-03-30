using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class GunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Weapon myWeaponScript = (Weapon)target;
        if(GUILayout.Button("Shoot"))
        {
            myWeaponScript.Shoot();
        }
    }
}
