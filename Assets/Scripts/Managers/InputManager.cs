using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    public Entity hoveredOn;
    public GameObject clickedOn;
    public EventSystem eventSystem;

    private Camera camera;
    private int terrainMask = 1 << 11;

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

        camera = Camera.main;
    }

    public void Update()
    {
        // need this since Unity doesn't pass RClick to game objects
        if (Input.GetMouseButtonDown(1))
        {
            UIManager.instance.HideInteractionPane();
            if (hoveredOn)
            {
                if (!eventSystem.IsPointerOverGameObject())
                    TheGameManager.GameManager.instance.EntityClicked(hoveredOn, 1);
            }
            else
            {
                Vector3 pos = new Vector3();
                if (GetMouseTerrainCoords(out pos))
                    TheGameManager.GameManager.instance.TerrainClicked(pos);
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            TheGameManager.GameManager.instance.ActionKey();
        }
    }

    public void HoverObject(Entity e)
    {   
        hoveredOn = e;
    }

    public void ClickObject(Entity e)
    {
        if (!e) return;

        clickedOn = e.gameObject;
        int button = -1;
        if (Input.GetMouseButtonDown(0))
            button = 0;
        else if (Input.GetMouseButtonDown(1))
            button = 1;
        
        // don't pass clicks through if you're clicking UI
        if(!eventSystem.IsPointerOverGameObject())
            TheGameManager.GameManager.instance.EntityClicked(e, button);
    }

    public bool GetMouseTerrainCoords(out Vector3 v)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, terrainMask, QueryTriggerInteraction.Ignore))
        {
            v = hit.point;
            return true;
        }
        v = new Vector3();
        return false;
    }
}
