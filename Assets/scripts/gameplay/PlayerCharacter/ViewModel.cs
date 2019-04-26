using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class ViewModel : PlayerModuleBase
    {
        public Transform playerModelTransform;
        public string layerVisibleName, layerInvisibleName;

        public override void Initialize()
        {
            base.Initialize();

            // foreach (Renderer i in playerModelTransform.GetComponentsInChildren<Renderer>())
            // {
            //     Collider c = i.GetComponent<Collider>();
            //     if (c)
            //         i.enabled = !playerCharacter.isLocalPlayer;

            //     i.gameObject.layer = LayerMask.NameToLayer(playerCharacter.isLocalPlayer? layerInvisibleName : layerInvisibleName);
            // }

            playerModelTransform.gameObject.SetActive(!isLocalPlayer);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnClientUpdate()
        {
            base.OnClientUpdate();
        }
    }

}