using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Primeval.Networking;

namespace Primeval.PlayerCharacter
{
    public class AudioPlayer : PlayerModuleBase
    {
        public AudioClip[] soundBank; //multiplayer only
        
        public AudioSource source;

        public override void Initialize()
        {
            base.Initialize();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void PlaySound(AudioClip clip, bool networked = false)
        {
            if (networked && photonView.isMine)
            {
                CmdPlay(clip.name);
                return;
            }
            source.PlayOneShot(clip);
        }

        //[Command]
        public void CmdPlay(string n)
        {
            photonView.RPC("RpcPlay", PhotonTargets.All, n);
        }

        [PunRPC]
        public void RpcPlay(string n)
        {
            foreach (AudioClip i in soundBank)
            {
                if (i.name.ToLower() == n.ToLower())
                {
                    source.PlayOneShot(i);
                    break;
                }
            }
        }
    }
}