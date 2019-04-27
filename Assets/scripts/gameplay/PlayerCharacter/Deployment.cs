using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class Deployment : PlayerModuleBase
    {
        public Vector2 dropOffPoint { get; private set; }
        public bool dropping { get; private set; }
        public bool disabled { get; private set; }
        public Transform dropPodModel;

        public LayerMask dropCollisionMask;

        RaycastHit hitInfo;

        public float altitude
        { get; private set; }

        public float startHeight;

        public NetworkTransform networkTransform;

        public float duration;
        public float time { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void OnDeploy()
        {
            time = 0;

            disabled = false;
            dropping = true;
            dropPodModel.gameObject.SetActive(true);
        }

        public void OnLand()
        {
            dropping = false;
        }

        public void OnOpen()
        {
            disabled = true;
            dropPodModel.gameObject.SetActive(false); //TODO: animate
        }
    }
}