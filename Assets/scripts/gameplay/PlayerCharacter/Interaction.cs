using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Item;
using Primeval.ViewModels;

namespace Primeval.PlayerCharacter
{
    public class Interaction : PlayerModuleBase
    {
        public float interactionDistance;
        public LayerMask interactionLayers;

        Interactable currentInteractable;
        RaycastHit hitInfo;


        public override void Initialize()
        {
            base.Initialize();
            VMInteraction.instance.Set("", 0, 0, false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            VMInteraction.instance.Set("", 0, 0, false);

            Ray r = CameraManager.instance.mainCamera.ViewportPointToRay(Vector3.one/2);
            Physics.Raycast(r, out hitInfo, interactionDistance, interactionLayers);

            if (hitInfo.collider)
            {
                Debug.DrawLine(r.origin, hitInfo.point, Color.red);
                Interactable i = hitInfo.collider.GetComponentInParent<Interactable>();
                currentInteractable = i;
                if (currentInteractable)
                {
                    ItemBase b = currentInteractable.GetComponent<ItemBase>();
                    if (b)
                    {
                        VMInteraction.instance.Set(b.itemData.name, b.itemData.weight*b.currentAmount, b.currentAmount, true);   
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currentInteractable)
                    {
                        playerCharacter.CmdInteract(currentInteractable.gameObject);
                    }else{
                        Outpost outpost = hitInfo.collider.GetComponentInParent<Outpost>();
                        if (outpost)
                        {
                            outpost.DropFuel();
                        }
                    }
                }
            }else{
                currentInteractable = null;
            }
        }
    }

}