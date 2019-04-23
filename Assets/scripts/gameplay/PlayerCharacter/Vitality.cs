using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
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

        public override void Initialize()
        {
            base.Initialize();

            currentHitPoints = maxHitPoints;
            currentArmorPoints = maxArmorPoints;

            OnChangeHitpoints();
            OnChangeArmorPoints();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }


        public void Damage(int amount)
        {
            //TODO: tps damage effect
            if (!isLocalPlayer)
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
            if (!isLocalPlayer)
                return;
                
            currentHitPoints += amount;
            OnChangeHitpoints();
        }
        
        public void HealArmorPoints(int amount)
        {
            //TODO: heal effect
            if (!isLocalPlayer)
                return;
                
            currentArmorPoints += amount;
            OnChangeArmorPoints();
        }

        public void OnChangeHitpoints()
        {
            currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitPoints);
            VMVitality.instance.HP = currentHitPoints.ToString();

            if (currentHitPoints <= 0)
            {
                if (isLocalPlayer)
                    CmdKillPlayer();
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
            timeKilled = NetworkTime.time;

            //TODO: animate death
        }

        public void OnRespawn(Vector3 position)
        {
            isDead = false;

            if (isLocalPlayer)
            {
                currentHitPoints = maxHitPoints;
                currentArmorPoints = maxArmorPoints / 4;

                OnChangeHitpoints();
                OnChangeArmorPoints();
            }

            
            //TODO: animate respawn
        }

        public int GetArmorReduction(int damage)
        {
            float x = damage;
            x = Mathf.Lerp(damage, damage / 3, currentArmorPoints / maxArmorPoints);
            return Mathf.RoundToInt(x);
        }


        [Command]
        public void CmdDamage(int amt)
        {
            RpcDamage(amt);
        }

        [ClientRpc]
        public void RpcDamage(int amt)
        {
            Damage(amt);
        }

        [Command]
        public void CmdKillPlayer()
        {
            RpcKillPlayer();
        }

        [ClientRpc]
        public void RpcKillPlayer()
        {
            OnKilled();
        }
    }
}