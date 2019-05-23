using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Primeval.Networking;
using Primeval.Item;

namespace Primeval.PlayerCharacter
{
    public class PlayerCharacter : PunBehaviour
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
        public Deployment deploymentModule;
        public AudioPlayer audioPlayerModule;

        public void OnStartLocalPlayer()
        {
            if (photonView.isMine)
                myPlayer = this;

            if (PhotonNetwork.isMasterClient)
            {
                hostPlayer = this;
                // NetworkManagerExt.instance.OnStartLevel();
            }

            foreach (PlayerModuleBase i in modules)
            {
                i.Initialize();
            }

            gameObject.name = "PC-" + photonView.viewID.ToString();

            CmdSetScore(0);
            GameManager.DeployPlayer();
        }

        void Awake()
        {
            modules = GetComponentsInChildren<PlayerModuleBase>(true);

            if (photonView.isMine)
            {
                OnStartLocalPlayer();
            }
        }

        void OnDestroy()
        {
            // if (photonView.isMine)
            //     inventoryFPSModelModule.ShowItemModel(null, null);
        }


        void Update()
        {
            foreach (PlayerModuleBase i in modules)
            {
                if (!i.initialized || !i.isActive)
                    continue;

                if (photonView.isMine)
                {
                    i.OnUpdate();
                }
                else
                {
                    i.OnClientUpdate();
                }
            }
        }

        public void SetInput(bool e)
        {
            bool enableInput = e;
            if (vitalityModule.isDead)
                enableInput = false;

            movementModule.isActive = enableInput;
            mouselookModule.isActive = enableInput;
            stanceModule.isActive = enableInput;
            inventoryModule.isActive = enableInput;
        }

        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up * 0.5f), 0.5f);
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up), 0.5f);
        //     Gizmos.DrawWireSphere(transform.position + (Vector3.up * 1.5f), 0.5f);
        // }



        //RPCS and COMMANDS
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }


        //[Command]
        public void CmdInteract(int target)
        {
            photonView.RPC("RpcInteract", PhotonTargets.All, target);
        }

        [PunRPC]
        public void RpcInteract(int target)
        {
            GameObject g = PhotonView.Find(target).gameObject;
            g.GetComponent<Interactable>().OnInteract(gameObject);
        }

        //[Command]
        public void CmdRemove(int n, int x)
        {
            photonView.RPC("RpcRemove", PhotonTargets.All, n, x);
        }

        [PunRPC]
        public void RpcRemove(int x, int amount)
        {
            GameObject g = PhotonView.Find(x).gameObject;
            ItemBase n = g.GetComponent<ItemBase>();
            n.currentAmount = amount;
            print("current amount: " + n.currentAmount);

            if (amount <= 0)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.Destroy(g);
                }
            }
        }

        //[Command]
        public void CmdInflictDamage(int amt, int to)
        {
            photonView.RPC("RpcInflictDamage", PhotonTargets.All, amt, to);
        }

        [PunRPC]
        public void RpcInflictDamage(int amt, int g)
        {
            GameObject to = PhotonView.Find(g).gameObject;
            PlayerCharacter p = to.GetComponent<PlayerCharacter>();
            if (p.photonView.isMine)
                p.vitalityModule.CmdDamage(amt);
        }

        //[Command]
        public void CmdSetScore(int x)
        {
            photonView.RPC("RpcSetScore", PhotonTargets.All, x);
        }

        [PunRPC]
        public void RpcSetScore(int x)
        {
            myScore = x;
        }

        //Room Properties
        //[Command]
        public void CmdGameTime(float t)
        {
            photonView.RPC("RpcGameTime", PhotonTargets.All, t);
        }
        [PunRPC]
        public void RpcGameTime(float t)
        {
            GameManager.gameTime = t;
        }



        public static PlayerCharacter FindByID(int x)
        {
            foreach (PlayerCharacter i in GameObject.FindObjectsOfType<PlayerCharacter>())
            {
                if (i.photonView.viewID == x)
                    return i;
            }

            return null;
        }
    }

}