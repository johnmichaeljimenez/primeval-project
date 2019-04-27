using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval.Networking;
using Primeval.Item;

namespace Primeval.PlayerCharacter
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public static PlayerCharacter hostPlayer;
        public static PlayerCharacter myPlayer;

        //TODO: move scores to game manager
        public int myScore
        {
            get;
            private set;
        }

        PlayerModuleBase[] modules;

        public Stance stanceModule;
        public Inventory inventoryModule;
        public InventoryFPSModel inventoryFPSModelModule;
        public Movement movementModule;
        public Mouselook mouselookModule;
        public Vitality vitalityModule;
        public ItemEffects itemEffectsModule;
        public CharacterAnimator characterAnimatorModule;

        public override void OnStartLocalPlayer()
        {
            if (isLocalPlayer)
                myPlayer = this;

            if (isServer)
            {
                hostPlayer = this;
                ((NetworkManagerExt)(NetworkManager.singleton)).OnStartLevel();
            }

            foreach (PlayerModuleBase i in modules)
            {
                i.Initialize();
            }

            gameObject.name = "PC-" + netId.ToString();

            CmdSetScore(0);
        }

        void Awake()
        {
            modules = GetComponentsInChildren<PlayerModuleBase>(true);
        }

        void OnDestroy()
        {
            if (isLocalPlayer)
                inventoryFPSModelModule.ShowItemModel(null, null);
        }


        void Update()
        {
            foreach (PlayerModuleBase i in modules)
            {
                if (!i.initialized)
                    continue;

                if (isLocalPlayer)
                {
                    i.OnUpdate();
                }
                else
                {
                    i.OnClientUpdate();
                }
            }
        }

        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up * 0.5f), 0.5f);
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up), 0.5f);
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up * 1.5f), 0.5f);
        // }



        //RPCS and COMMANDS


        [Command]
        public void CmdInteract(GameObject target)
        {
            RpcInteract(target);
        }

        [ClientRpc]
        public void RpcInteract(GameObject target)
        {
            target.GetComponent<Interactable>().OnInteract(gameObject);
        }

        [Command]
        public void CmdRemove(GameObject n, int x)
        {
            RpcRemove(n, x);
        }

        [ClientRpc]
        public void RpcRemove(GameObject g, int amount)
        {
            ItemBase n = g.GetComponent<ItemBase>();
            n.currentAmount = amount;
            print("current amount: " + n.currentAmount);

            if (amount <= 0)
            {
                if (isServer)
                {
                    NetworkServer.Destroy(g);
                }
            }
        }

        [Command]
        public void CmdInflictDamage(int amt, GameObject to)
        {
            RpcInflictDamage(amt, to);
        }

        [ClientRpc]
        public void RpcInflictDamage(int amt, GameObject to)
        {
            PlayerCharacter p = to.GetComponent<PlayerCharacter>();
            if (p.isLocalPlayer)
                p.vitalityModule.CmdDamage(amt);
        }

        [Command]
        public void CmdSetScore(int x)
        {
            RpcSetScore(x);
        }

        [ClientRpc]
        public void RpcSetScore(int x)
        {
            myScore = x;
        }

        //Room Properties
        [Command]
        public void CmdGameTime(float t)
        {
            RpcGameTime(t);
        }
        [ClientRpc]
        public void RpcGameTime(float t)
        {
            GameManager.gameTime = t;
        }



        public static PlayerCharacter FindByID(uint x)
        {
            foreach (PlayerCharacter i in GameObject.FindObjectsOfType<PlayerCharacter>())
            {
                if (i.netId == x)
                    return i;
            }

            return null;
        }
    }

}