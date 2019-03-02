using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    public Entity hoveredOn;
    public GameObject clickedOn;

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
        if (!hoveredOn) return;

        // need this since Unity doesn't pass RClick to game objects
        if (Input.GetMouseButtonDown(1))
        {
            TheGameManager.GameManager.instance.EntityClicked(hoveredOn, 1);
        }
    }

    public void HoverObject(Entity e)
    {
        if (!e) return;
        
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

        TheGameManager.GameManager.instance.EntityClicked(e, button);
    }
}
