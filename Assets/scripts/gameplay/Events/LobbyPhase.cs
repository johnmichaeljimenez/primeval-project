using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;

namespace Primeval.Events
{
    public class LobbyPhase : TimedEventBase
    {
        public string notificationText;

        public override void OnStart()
        {
            base.OnStart();
            //TODO: start game events
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnSecondTick()
        {
            base.OnSecondTick();
            //TODO: notify
        }

        public override void OnExpire()
        {
            base.OnExpire();
            //TODO: deploy players
        }
    }
}