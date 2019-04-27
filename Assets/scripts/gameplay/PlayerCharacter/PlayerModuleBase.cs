using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class PlayerModuleBase : NetworkBehaviour
    {

        private PlayerCharacter _playerCharacter;

        public PlayerCharacter playerCharacter
        {
            get
            {
                if (!_playerCharacter)
                    _playerCharacter = GetComponentInParent<PlayerCharacter>();

                return _playerCharacter;
            }
        }

        public bool isActive;

        [HideInInspector]
        public bool initialized;
        public virtual void Initialize()
        {
            initialized = true;
            isActive = true;
        }

        public virtual void OnUpdate()
        {
            if (!initialized)
                return;
        }

        public virtual void OnClientUpdate()
        {
            if (!initialized)
                return;
        }
    }
}