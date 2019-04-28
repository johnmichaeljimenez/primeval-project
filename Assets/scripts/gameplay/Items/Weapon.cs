using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Data;
using Primeval.PlayerCharacter;

namespace Primeval.Item
{
    public class Weapon : MonoBehaviour
    {
        public static Weapon interrupt
        {
            get;
            private set;
        }

        public ItemFPSModel itemFPSModel
        {
            get;
            private set;
        }
        public Animator weaponAnimator;
        public Transform firingEffect;
        float movementBlend = 0;

        public static bool aiming { get; private set; }

        InventoryItem refItem
        {
            get
            {
                return itemFPSModel.referenceInventoryItem;
            }
        }

        WeaponData weaponData
        {
            get
            {
                return refItem.data as WeaponData;
            }
        }

        public LayerMask bulletRaycastMask;
        RaycastHit hitInfo;

        PlayerCharacter.PlayerCharacter myPlayer;
        bool running, grounded, moving;
        float t;

        public virtual void OnEnable()
        {
            interrupt = null;
            itemFPSModel = GetComponentInParent<ItemFPSModel>();

            SetState(WeaponStates.Ready);
            myPlayer = PlayerCharacter.PlayerCharacter.myPlayer;
        }
        public virtual void Update()
        {
            if (myPlayer)
            {
                running = myPlayer.movementModule.isRunning && myPlayer.movementModule.runDelay >= 0.3f;
                grounded = myPlayer.movementModule.isGrounded;
                moving = myPlayer.movementModule.inputDirection.sqrMagnitude > 0;

                t = aiming? -1 : (!grounded ? 0 : running && moving ? 2 : moving ? 1 : 0);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                int rs = itemFPSModel.referenceInventoryItem.GetReservedAmmo();
                if (rs > 0 && refItem.currentAmmo < weaponData.ammo1Capacity)
                    SetState(WeaponStates.Reload);
            }

            if (InputSystem.mouseLeftHold && !running)
            {
                if (refItem.currentAmmo > 0)
                    SetState(WeaponStates.Fire);
            }

            if (InputSystem.mouseLeftClicked && refItem.currentAmmo <= 0)
                PlayerCharacter.PlayerCharacter.myPlayer.audioPlayerModule.PlaySound(weaponData.firingDryClip);

            if (weaponData.canAim && !running)
            {
                aiming = InputSystem.mouseRightHold && weaponAnimator.GetInteger("state") == (int)WeaponStates.Idle;
            }

            CalculateMovementBlend();
        }

        public void CalculateMovementBlend()
        {
            movementBlend = Mathf.Lerp(movementBlend, t, Time.deltaTime * (aiming? 30 : 5));

            weaponAnimator.SetFloat("movement", movementBlend);
        }

        public void SetState(WeaponStates w)
        {
            if (interrupt)
                return;

            weaponAnimator.SetInteger("state", (int)w);
        }

        //events
        public virtual void OnSwitch()
        {
            interrupt = this;
        }
        public virtual void OnReady()
        {
            interrupt = null;
            SetState(WeaponStates.Idle);
        }
        public virtual void OnHide() { }
        public virtual void OnFireStart()
        {
            interrupt = this;
            myPlayer.characterAnimatorModule.CmdAnimatorTrigger("fireWeapon");
        }
        public virtual void OnApplyFire()
        {
            if (!refItem.hasAmmo)
            {
                return;
            }

            PlayerCharacter.PlayerCharacter.myPlayer.audioPlayerModule.PlaySound(weaponData.firingClip);

            refItem.SetAmmoCount(refItem.currentAmmo - 1);

            if (weaponData.weaponType == WeaponTypes.Firearm)
            {
                Vector3 r1, r2, acc;
                acc = Random.insideUnitCircle * weaponData.spread * 2;
                r1 = CameraManager.instance.mainCamera.transform.position;
                r2 = r1 + (CameraManager.instance.mainCamera.transform.forward * 1000);
                r2 += acc;

                if (Physics.Linecast(r1, r2, out hitInfo, bulletRaycastMask))
                {
                    Debug.DrawLine(r1, hitInfo.point, Color.red, 0.4f);

                    PlayerCharacter.PlayerCharacter p = hitInfo.collider.GetComponentInParent<PlayerCharacter.PlayerCharacter>();
                    if (!p)
                        return;

                    PlayerCharacter.PlayerCharacter.myPlayer.CmdInflictDamage(weaponData.damage, p.gameObject);
                }
            }
        }
        public virtual void OnFireEnd()
        {
            interrupt = null;
            SetState(WeaponStates.Idle);
        }
        public virtual void OnReloadStart()
        {
            interrupt = this;
            myPlayer.characterAnimatorModule.CmdAnimatorTrigger("reloadWeapon");
        }
        public virtual void OnApplyReload()
        {
            refItem.ReloadAmmo();
        }
        public virtual void OnReloadEnd()
        {
            interrupt = null;
            SetState(WeaponStates.Idle);
        }

        public enum WeaponStates
        {
            Ready,
            Idle,
            Fire,
            Reload
        }
    }
}