using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Mirror;

namespace Primeval.Events
{
    public class TimedEventBase
    {
        public float duration;
        public bool isActive { get; private set; }
        public TimedEventBase nextEvent;

        public float currentTime { get; set; }
        float lastTime;

        public virtual void OnStart()
        {
            isActive = true;
            currentTime = duration;
            lastTime = currentTime;
        }
        public virtual void OnUpdate()
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= lastTime - 1)
            {
                lastTime -= 1;
                OnSecondTick();

                if (currentTime <= 0)
                    OnExpire();
            }
        }
        public virtual void OnSecondTick() { }
        public virtual void OnExpire()
        {
            isActive = false;
            ExecuteNextEvent();
        }

        public void ExecuteNextEvent()
        {
            if (nextEvent == null)
                return;

            nextEvent.ExecuteEvent();
        }

        public void ExecuteEvent()
        {
            OnStart();
        }

    }
}