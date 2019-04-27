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
        
        public float lookX
        {
            get
            {
                return animator.GetFloat("lookX");
            }
            set
            {
                animator.SetFloat("lookX", value);
            }
        }
        
        public float lookY
        {
            get
            {
                return animator.GetFloat("lookY");
            }
            set
            {
                animator.SetFloat("lookY", value);
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
                float lx = 0;
                float ly = 0;
                
                if (playerCharacter.inventoryFPSModelModule.activeModel)
                {
                    wp = playerCharacter.inventoryFPSModelModule.activeModel.transform.GetSiblingIndex()+1;
                }

                lx = 0;//playerCharacter.mouselookModule.normalizedAngle.x;
                ly = playerCharacter.mouselookModule.normalizedAngle.y;

                CmdAnimate(mx, my, st, wp, oa, lx, ly);
            }
        }

        [Command]
        public void CmdAnimate(float mx, float my, int st, int wp, bool oa, float lx, float ly)
        {
            RpcAnimate(mx, my, st, wp, oa, lx, ly);
        }

        [ClientRpc]
        public void RpcAnimate(float mx, float my, int st, int wp, bool oa, float lx, float ly)
        {
            movementX = mx;
            movementY = my;
            stance = st;
            onAir = !oa;
            lookX = lx;
            lookY = ly;


            animator.SetLayerWeight(1, wp > 0? 1 : 0);
            animator.SetLayerWeight(2, wp > 0? 1 : 0);
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