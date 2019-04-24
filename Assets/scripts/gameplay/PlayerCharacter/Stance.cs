using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class Stance : PlayerModuleBase
    {
        public bool interrupt;

        public Stances currentStance;

        public override void Initialize()
        {
            base.Initialize();

            CmdChangeStance(Stances.Stand);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKeyDown(KeyCode.C))
                CmdChangeStance(isStanding ? Stances.Crouch : Stances.Stand);
        }

        [Command]
        public void CmdChangeStance(Stances s)
        {
            RpcChangeStance(s);
        }

        [ClientRpc]
        public void RpcChangeStance(Stances s)
        {
            currentStance = s;
            OnChangeStance(s);
        }

        void OnChangeStance(Stances s)
        {
            print("stance: " + s);
        }

        public bool isStanding
        {
            get
            {
                return currentStance == Stances.Stand;
            }
        }
        public bool isCrouching
        {
            get
            {
                return currentStance == Stances.Crouch;
            }
        }
    }
    

    public enum Stances
    {
        Stand,
        Crouch
    }
}
