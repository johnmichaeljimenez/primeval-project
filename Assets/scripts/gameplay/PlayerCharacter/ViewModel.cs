using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class ViewModel : PlayerModuleBase
    {
        public Transform playerModelTransform;

        public override void Initialize()
        {
            base.Initialize();

            playerModelTransform.gameObject.SetActive(!playerCharacter.isLocalPlayer);
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