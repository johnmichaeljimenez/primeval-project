using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval.Networking;

namespace Primeval.PlayerCharacter
{
    public class AudioPlayer : PlayerModuleBase
    {
        public AudioSource source;

        public override void Initialize()
        {
            base.Initialize();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void PlaySound(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }
    }
}