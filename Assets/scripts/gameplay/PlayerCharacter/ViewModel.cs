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

            foreach (Renderer i in playerModelTransform.GetComponentsInChildren<Renderer>())
            {
                Collider c = i.GetComponent<Collider>();
                if (c)
                    i.enabled = !playerCharacter.photonView.isMine;

                i.gameObject.layer = LayerMask.NameToLayer(playerCharacter.photonView.isMine? layerInvisibleName : layerVisibleName);
            }

            // playerModelTransform.gameObject.SetActive(!photonView.isMine);
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