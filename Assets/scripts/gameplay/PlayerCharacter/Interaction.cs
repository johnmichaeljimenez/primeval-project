using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class Interaction : PlayerModuleBase
    {
        public float interactionDistance;
        public LayerMask interactionLayers;

        Interactable currentInteractable;
        RaycastHit hitInfo;

        public AudioClip interactionClip;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Ray r = CameraManager.instance.mainCamera.ViewportPointToRay(Vector3.one/2);
            Physics.Raycast(r, out hitInfo, interactionDistance, interactionLayers);

            if (hitInfo.collider)
            {
                Debug.DrawLine(r.origin, hitInfo.point, Color.red);
                Interactable i = hitInfo.collider.GetComponentInParent<Interactable>();
                currentInteractable = i;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currentInteractable)
                    {
                        playerCharacter.audioPlayerModule.PlaySound(interactionClip);
                        playerCharacter.CmdInteract(currentInteractable.gameObject);
                    }else{
                        Outpost outpost = hitInfo.collider.GetComponentInParent<Outpost>();
                        if (outpost)
                        {
                            playerCharacter.audioPlayerModule.PlaySound(interactionClip);
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