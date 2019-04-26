using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Mirror;

namespace Primeval.Events
{
    public class TimedEventManager : GenericSingletonClass<TimedEventManager>
    {
        public List<TimedEventBase> events;

        void Awake()
        {
            
        }

        void Update()
        {
            for (int i = events.Count - 1; i >= 0 ; i--)
            {
                TimedEventBase t = events[i];
                t.OnUpdate();
            }
        }
    }
}