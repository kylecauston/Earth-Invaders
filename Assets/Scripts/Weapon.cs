using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public GameObject muzzleFlash;  // a child of this, so it moves with the weapon
    public float flashLength = 1;

    private WaitForSeconds delay;

    // this is the muzzle flash for when not shooting from the weapon
    // ie. when showing muzzle flashes from buildings
    private GameObject copy;
    private bool usingCopy = false;

    public void Start()
    {
        delay = new WaitForSeconds(flashLength);
    }

    public void Shoot()
    {
        AnimateShot();
    }

    public void AnimateShot()
    {
        usingCopy = false;
        StartCoroutine(ShowFlash());
    }

    public void AnimateShot(Vector3 point, Vector3 target) // Add particle effects?
    {
        usingCopy = true;
        copy = Instantiate<GameObject>(muzzleFlash);
        Quaternion rot = Quaternion.LookRotation(target - point);
        Debug.Log(target - point);
        rot *= Quaternion.AngleAxis(90, Vector3.right);

        copy.transform.SetPositionAndRotation(point, rot);
        StartCoroutine(ShowFlash());
    }

    private IEnumerator ShowFlash()
    {
        Debug.Log("Showing flash.");
        if (usingCopy)
        {
            copy.SetActive(true);
        }
        else
        {
            muzzleFlash.SetActive(true);
        }
        
        yield return delay;
        
        Debug.Log("Hiding flash.");

        if (usingCopy)
        {
            Destroy(copy);
        }
        else
        {
            muzzleFlash.SetActive(false);
        }
    }
}