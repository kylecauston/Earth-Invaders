using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGameManager    // avoid using Unity's prebuilt GameManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        public Globals.Alignment playerAlignment = Globals.Alignment.Space;
        
        public Entity selectedEntity;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SelectEntity(Entity e)
        {
            selectedEntity = e;
            Debug.Log("Selected " + e.name);
        }

        public void EntityClicked(Entity e, int mouseButton)
        {
            // NEW SCHEME
            // Left click (0) is only ever select.
            //   Left click the selected unit unselects it.
            // Right click (1) opens the interactions menu.
            //   Right click with no selected unit does nothing.
            
            if (e == null)
                return;

            if(mouseButton == 0) // left click
            {
                SelectEntity(e);
            }
            else if (mouseButton == 1) // right click
            {
                if (!selectedEntity || selectedEntity.alignment != playerAlignment)
                    return;

                InteractionManager.instance.Interact(selectedEntity, e, 0);
            }
        }

        public void TerrainClicked(Vector3 click)
        {
            // When we click the terrain it generally denotes a movement action.

            // How do we check where on a Terrain we clicked?
            //      Raycast to the terrain!

            // if we're in spawn mode, we spawn the unit at the given location
            // make a SpawnManager?

            // if we can move this unit, tell it to move to the location
            if(selectedEntity && selectedEntity.alignment == playerAlignment)
            {
                // How do we want to do behavior? we need a behavior tree 
                // for stuff like:
                // "Interact with [Far Object x]"
                // is translated to
                // "Move to x" + "Interact with x"
                // Furthermore, not all entities can move. How do we check this?
                //selectedEntity.MoveTo(click);
            }
        }
    }
}