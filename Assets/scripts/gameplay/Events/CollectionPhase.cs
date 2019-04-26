using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Mirror;

namespace Primeval.Events
{
    public class CollectionPhase : TimedEventBase
    {
        public string notificationText;

        public override void OnStart()
        {
            base.OnStart();
            //TODO: spawn items
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnSecondTick()
        {
            base.OnSecondTick();
        }
    }
}