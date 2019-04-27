using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class CharacterAnimator : PlayerModuleBase
    {
        public Animator animator;
        public PlayerHandIK playerHandIK;
        public Transform tpsWeaponContainers;

        WeaponTPSModel[] weaponTPSModels;

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
            weaponTPSModels = tpsWeaponContainers.GetComponentsInChildren<WeaponTPSModel>(true);
        }

        int mCurrentWeaponTPS;
        int currentWeaponTPS
        {
            get{
                return mCurrentWeaponTPS;
            }
            set{
                if (mCurrentWeaponTPS != value)
                {
                    mCurrentWeaponTPS = value;
                    OnWeaponTPSChanged();
                }
            }
        }

        void Start()
        {
            SetRagdoll(false);
            OnWeaponTPSChanged();
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
                bool oa = playerCharacter.movementModule.isGrounded;
                float lx = 0;
                float ly = 0;
                
                if (playerCharacter.inventoryFPSModelModule.activeModel)
                {
                    currentWeaponTPS = playerCharacter.inventoryFPSModelModule.activeModel.transform.GetSiblingIndex()+1;
                }else
                {
                    currentWeaponTPS = 0;
                }

                lx = 0;//playerCharacter.mouselookModule.normalizedAngle.x;
                ly = playerCharacter.mouselookModule.normalizedAngle.y;

                CmdAnimate(mx, my, st, currentWeaponTPS, oa, lx, ly);
            }
        }

        void OnWeaponTPSChanged()
        {
            Transform h = null;

            int n = 0;
            foreach (WeaponTPSModel i in weaponTPSModels)
            {
                bool en = n == currentWeaponTPS-1;
                i.gameObject.SetActive(en);
                if (en)
                    h = i.leftHandHandle;
            }

            playerHandIK.SetIKHand(currentWeaponTPS > 0, h);
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
            animator.SetLayerWeight(3, wp > 0? 1 : 0);
        }

        

        [Command]
        public void CmdAnimatorTrigger(string n)
        {
            RpcAnimatorTrigger(n);
        }

        [ClientRpc]
        public void RpcAnimatorTrigger(string n)
        {
            animator.SetTrigger(n);
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