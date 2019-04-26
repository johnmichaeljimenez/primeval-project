using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class CharacterAnimator : PlayerModuleBase
    {
        public Animator animator;

        public float movementX
        {
            get
            {
                return animator.GetFloat("movementX");
            }
            set
            {
                animator.SetFloat("movementX", value);
            }
        }
        public bool onAir
        {
            get
            {
                return animator.GetBool("onAir");
            }
            set
            {
                animator.SetBool("onAir", value);
            }
        }
        public float movementY
        {
            get
            {
                return animator.GetFloat("movementY");
            }
            set
            {
                animator.SetFloat("movementY", value);
            }
        }
        public int stance
        {
            get
            {
                return animator.GetInteger("stance");
            }
            set
            {
                animator.SetInteger("stance", value);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            movementX = 0;
            movementY = 0;
        }

        void Start()
        {
            SetRagdoll(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (isLocalPlayer)
            {

                float y = playerCharacter.movementModule.inputDirection.z;

                float mx = playerCharacter.movementModule.inputDirection.x;
                float my = y + (y > 0 ? playerCharacter.movementModule.runDelay : 0);
                int st = (int)playerCharacter.stanceModule.currentStance;
                int wp = 0;
                bool oa = playerCharacter.movementModule.isGrounded;
                
                if (playerCharacter.inventoryFPSModelModule.activeModel)
                {
                    wp = playerCharacter.inventoryFPSModelModule.activeModel.transform.GetSiblingIndex()+1;
                }

                CmdAnimate(mx, my, st, wp, oa);
            }
        }

        [Command]
        public void CmdAnimate(float mx, float my, int st, int wp, bool oa)
        {
            RpcAnimate(mx, my, st, wp, oa);
        }

        [ClientRpc]
        public void RpcAnimate(float mx, float my, int st, int wp, bool oa)
        {
            movementX = mx;
            movementY = my;
            stance = st;
            onAir = !oa;


            animator.SetLayerWeight(1, wp > 0? 1 : 0);
        }

        public void SetRagdoll(bool enable)
        {
            foreach (Rigidbody i in GetComponentsInChildren<Rigidbody>())
            {
                i.isKinematic = !enable;
            }

            animator.enabled = !enable;
        }
    }
}