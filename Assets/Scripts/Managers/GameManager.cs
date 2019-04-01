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

        private int currency = 1000;

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
            if(selectedEntity == e)
            {
                e = null;
            }

            selectedEntity = e;
            if(ground)
            {
                UIManager.instance.SelectEntity(selectedEntity);
                CameraManager.instance.LockTo(selectedEntity);
            }

            if(selectedEntity != null)
                SpawnManager.instance.SetSelected(-1);
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

                // How do we want to do behavior? we need a behavior tree 
                // for stuff like:
                // "Interact with [Far Object x]"
                // is translated to
                // "Move to x" + "Interact with x"
                // Furthermore, not all entities can move. How do we check this?

                SpawnManager.instance.SetSelected(-1);
                AgentAI ai = selectedEntity.GetComponent<AgentAI>();
                Interaction interaction = InteractionManager.instance.GetInteraction(selectedEntity, e, 0);
                if (ai && interaction != null)
                    ai.AssignTask(interaction, e.gameObject);
                
            }
        }

        public void TerrainClicked(Vector3 click)
        {
            // When we click the terrain it generally denotes a movement action.
            int i = SpawnManager.instance.GetSelected();
            if (i != -1 && SpawnManager.instance.spawnables[i].cost < currency)
            {
                SpawnManager.instance.Spawn(click);
                return;
            }

            // if we can move this unit, tell it to move to the location
            if(CanControl(selectedEntity))
            {
                // detect if the entity can move
                CanMove movement = selectedEntity.GetComponent<CanMove>();
                if(movement)
                {
                    CanAttack attack = selectedEntity.GetComponent<CanAttack>();
                    if(attack)
                    {
                        attack.target = null;
                    }
                    movement.MoveTo(click);
                }
            }
        }

        public void ActionKey()
        {
            //SpawnManager.instance.SetSelected(0);
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

        public void SpendResource(int amount)
        {
            currency = Mathf.Max(0, currency - amount);
        }
    }
}