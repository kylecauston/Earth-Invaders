using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Material ground = null;

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
    }

    public void Update()
    {

        TheGameManager.GameManager gm = TheGameManager.GameManager.instance;
        if (gm.selectedEntity)
        {
            ground.SetVector("_Center", gm.selectedEntity.transform.position);
            ground.SetFloat("_Rotation", gm.selectedEntity.transform.rotation.eulerAngles.y);
        }
    }
}
