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
        public Material ground = null;

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
            if(ground && selectedEntity)
            {
                ground.SetVector("_Center", selectedEntity.transform.position);
                ground.SetFloat("_Rotation", selectedEntity.transform.rotation.eulerAngles.y);
                
                ground.SetColor("_BandColor", (selectedEntity.alignment == playerAlignment) ? Color.blue : selectedEntity.alignment == Globals.Alignment.Neutral ? Color.white : Color.red);
                ground.SetInt("_Shape", (int) selectedEntity.ringShape);
                ground.SetFloat("_Width", selectedEntity.ringRadius);
                ground.SetFloat("_BandWidth", selectedEntity.bandWidth);

                CameraManager.instance.LockTo(selectedEntity.gameObject);
            }
            else
            {
                ground.SetInt("_BandWidth", 0);
            }
        }

        private bool CanControl(Entity e)
        {
            return (e && e.alignment == playerAlignment);
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
                if (!(CanControl(selectedEntity)))
                    return;

                InteractionManager.instance.Interact(selectedEntity, e, 0);
            }
        }

        public void TerrainClicked(Vector3 click)
        {
            // When we click the terrain it generally denotes a movement action.
            

            // if we're in spawn mode, we spawn the unit at the given location
            // make a SpawnManager?

            // if we can move this unit, tell it to move to the location
            if(CanControl(selectedEntity))
            {
                // How do we want to do behavior? we need a behavior tree 
                // for stuff like:
                // "Interact with [Far Object x]"
                // is translated to
                // "Move to x" + "Interact with x"
                // Furthermore, not all entities can move. How do we check this?

                // detect if the entity can move
                CanMove movement = selectedEntity.GetComponent<CanMove>();
                if(movement)
                {
                    movement.MoveTo(click);
                }
            }
        }

        public void ActionKey()
        {
            if(CanControl(selectedEntity))
            {
                Building b = selectedEntity.gameObject.GetComponent<Building>();
                if(!b)
                {
                    Transport t = selectedEntity.gameObject.GetComponent<Transport>();
                    t.RemoveEntity(0);
                }
                else
                {
                    b.RemoveEntity(0);
                }
            }
        }
    }
}