using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Primeval.ViewModels;

namespace Primeval.PlayerCharacter
{
    public class Vitality : PlayerModuleBase
    {
        public int maxHitPoints = 100, maxArmorPoints = 100;
        public int currentHitPoints
        {
            get;
            private set;
        }

        public int currentArmorPoints
        {
            get;
            private set;
        }
        public bool isDead
        {
            get;
            private set;
        }
        public double timeKilled
        {
            get;
            private set;
        }

        public float minRespawnTime;
        public float respawnAddMultiplier;

        int deathCount;

        float respawnTime;

        public override void Initialize()
        {
            base.Initialize();

            deathCount = 0;
            respawnTime = 0;
            currentHitPoints = maxHitPoints;
            currentArmorPoints = maxArmorPoints;

            OnChangeHitpoints();
            OnChangeArmorPoints();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (photonView.isMine)
            {
                if (isDead)
                {
                    respawnTime += Time.deltaTime;
                    if (respawnTime >= minRespawnTime + (minRespawnTime * (deathCount * respawnAddMultiplier)))
                    {
                        respawnTime = 0;
                        deathCount += 1;

                        CmdRespawn();
                    }
                }
            }
        }


        public void Damage(int amount)
        {
            //TODO: tps damage effect
            if (!photonView.isMine)
                return;

            int output = GetArmorReduction(amount);
            currentArmorPoints -= amount;
            currentHitPoints -= output;
            OnChangeHitpoints();
            OnChangeArmorPoints();


            //TODO: stagger effect
        }

        public void HealHitPoints(int amount)
        {
            //TODO: heal effect
            if (!photonView.isMine)
                return;

            currentHitPoints += amount;
            OnChangeHitpoints();
        }

        public void HealArmorPoints(int amount)
        {
            //TODO: heal effect
            if (!photonView.isMine)
                return;

            currentArmorPoints += amount;
            OnChangeArmorPoints();
        }

        public void OnChangeHitpoints()
        {
            currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitPoints);

            if (photonView.isMine)
            {
                VMVitality.instance.HP = currentHitPoints.ToString();
                if (currentHitPoints <= 0)
                {
                    CmdKillPlayer();
                }
            }
        }

        public void OnChangeArmorPoints()
        {
            currentArmorPoints = Mathf.Clamp(currentArmorPoints, 0, maxArmorPoints);
            VMVitality.instance.AP = currentArmorPoints.ToString();
        }

        public void OnKilled()
        {
            if (isDead)
                return;

            isDead = true;
            timeKilled = GameManager.gameTime;

            //TODO: animate death
            playerCharacter.characterAnimatorModule.SetRagdoll(true);

            if (photonView.isMine)
            {
                playerCharacter.inventoryFPSModelModule.ShowItemModel(null, null);
                respawnTime = 0;
                playerCharacter.SetInput(false);
            }
        }

        public void OnRespawn()
        {
            isDead = false;

            if (photonView.isMine)
            {
                currentHitPoints = maxHitPoints;
                currentArmorPoints = maxArmorPoints / 4;

                OnChangeHitpoints();
                OnChangeArmorPoints();
                GameManager.DeployPlayer();
            }


            //TODO: animate respawn
            playerCharacter.characterAnimatorModule.SetRagdoll(false);
        }

        public int GetArmorReduction(int damage)
        {
            float x = damage;
            x = Mathf.Lerp(damage, damage / 3, currentArmorPoints / maxArmorPoints);
            return Mathf.RoundToInt(x);
        }


        public void CmdDamage(int amt)
        {
            photonView.RPC("RpcDamage", PhotonTargets.All, amt);
        }

        [PunRPC]
        public void RpcDamage(int amt)
        {
            Damage(amt);
        }

        //[Command]
        public void CmdKillPlayer()
        {
            photonView.RPC("RpcKillPlayer", PhotonTargets.All);
        }

        [PunRPC]
        public void RpcKillPlayer()
        {
            OnKilled();
        }


        //[Command]
        public void CmdRespawn()
        {
            photonView.RPC("RpcRespawn", PhotonTargets.All);
        }

        [PunRPC]
        public void RpcRespawn()
        {
            OnRespawn();
        }
    }
}