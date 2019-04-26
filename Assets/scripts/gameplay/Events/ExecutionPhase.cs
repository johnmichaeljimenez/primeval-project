using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Mirror;

namespace Primeval.Events
{
    public class ExecutionPhase : TimedEventBase
    {
        public string notificationText;

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnSecondTick()
        {
            base.OnSecondTick();
        }

        public override void OnExpire()
        {
            //TODO: eliminate lowest player
            base.OnExpire();
        }
    }
}