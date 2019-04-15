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

        private Entity interactionTarget;

        public int currency = 1000;

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

        public void Start()
        {
            UIManager.instance.UpdateCurrencyWindow(currency);
        }

        public Entity GetSelectedEntity()
        {
            return selectedEntity;
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
                
                SpawnManager.instance.SetSelected(-1);
                interactionTarget = e;
                UIManager.instance.ShowInteractionPane(selectedEntity, e);
            }
        }

        public void InteractionSelected(int i)
        {
            UIManager.instance.HideInteractionPane();
            AgentAI ai = selectedEntity.GetComponent<AgentAI>();
            Interaction interaction = InteractionManager.instance.GetInteraction(selectedEntity, interactionTarget, i);
            if (ai && interaction != null)
                ai.AssignTask(interaction, interactionTarget.gameObject);

            interactionTarget = null;
        }

        public void TerrainClicked(Vector3 click)
        {
            // When we click the terrain it generally denotes a movement action.


            int i = SpawnManager.instance.GetSelected();
            if (i != -1)
            {
                if (SpawnManager.instance.spawnables[i].cost <= currency)
                {
                    if (SpawnManager.instance.preview.CanSpawn())
                    {
                        SpawnManager.instance.Spawn(click);
                        SpendResource(SpawnManager.instance.spawnables[i].cost);
                    }
                }
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
            if(CanControl(selectedEntity))
            {
                Building b = selectedEntity.gameObject.GetComponent<Building>();
                if(!b)
                {
                    Transport t = selectedEntity.gameObject.GetComponent<Transport>();
                    if (t)
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
            UIManager.instance.UpdateCurrencyWindow(currency);
        }
    }
}