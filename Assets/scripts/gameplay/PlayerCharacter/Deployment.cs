﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Photon;
using Primeval.ViewModels;

namespace Primeval.PlayerCharacter
{
    public class Deployment : PlayerModuleBase
    {
        public Vector2 dropOffPoint { get; private set; }
        public bool dropping { get; private set; }
        public bool disabled { get; private set; }
        public Transform dropPodModel;

        public AnimationCurve gravityCurve;

        public LayerMask dropCollisionMask;

        RaycastHit hitInfo;

        public float altitude
        { get; private set; }

        public float startHeight;

        public PhotonTransformView networkTransform;

        public float duration;
        public float time { get; private set; }

        public AudioClip deploySound, landingSound, openSound;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        void Update()
        {
            if (disabled)
            {
                if (photonView.isMine)
                {
                    if (Input.GetKeyDown(KeyCode.P))
                        Deploy(Vector2.zero);//TODO: remove this (test only)
                }
                return;
            }

            if (dropping)
            {
                if (photonView.isMine)
                {
                    float t = gravityCurve.Evaluate(time / duration);
                    altitude = Mathf.Lerp(startHeight, hitInfo.point.y, t);
                    VMDeployment.instance.Altitude = t;
                    time += Time.deltaTime;
                    if (time >= duration)
                    {
                        time = duration; //TODO: fix end point for non-local players
                        Land();
                    }

                    CmdPosition(GetPoint(altitude));
                }
            }
            else
            {
                if (photonView.isMine)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        CmdOpen();
                    }
                }
            }
        }

        public Vector3 GetPoint(float y)
        {
            return new Vector3(dropOffPoint.x, y, dropOffPoint.y);
        }

        public void OnDeploy(Vector2 p)
        {
            print("deploying: " + playerCharacter.name);

            time = 0;
            disabled = false;
            dropping = true;

            networkTransform.enabled = false;
            transform.position = GetPoint(startHeight);

            if (photonView.isMine)
            {
                playerCharacter.audioPlayerModule.PlaySound(deploySound, true);
                Physics.Raycast(GetPoint(startHeight / 2), Vector3.down, out hitInfo, startHeight, dropCollisionMask);
                print("target location: " + GetPoint(hitInfo.point.y));
                VMDeployment.instance.TargetHeight = hitInfo.point.y.ToString("F1");
                VMDeployment.instance.StartingHeight = startHeight.ToString("F1");
            }
            dropPodModel.gameObject.SetActive(true);
            playerCharacter.SetInput(false);
        }

        public void OnLand()
        {
            if (photonView.isMine)
                transform.position = GetPoint(hitInfo.point.y);
            print("landing: " + playerCharacter.name);
            dropping = false;
            //TODO: play impact

            if (photonView.isMine)
            {
                playerCharacter.audioPlayerModule.PlaySound(landingSound, true);
            }
        }

        public void OnOpen()
        {
            //TODO: add interactable switch instead of locking input upon landing
            print("opening: " + playerCharacter.name);
            networkTransform.enabled = true;
            disabled = true;
            dropPodModel.gameObject.SetActive(false); //TODO: animate
            playerCharacter.SetInput(true);

            if (photonView.isMine)
            {
                playerCharacter.audioPlayerModule.PlaySound(openSound, true);
            }
        }

        public void Deploy(Vector2 point)
        {
            CmdDeploy(point);
        }

        public void Land()
        {
            CmdLand();
        }

        public void Open()
        {
            CmdOpen();
        }

        //[Command]
        public void CmdDeploy(Vector2 p)
        {
            photonView.RPC("RpcDeploy", PhotonTargets.All, p);
        }

        [PunRPC]
        public void RpcDeploy(Vector2 p)
        {
            OnDeploy(p);
        }


        //[Command]
        public void CmdLand()
        {
            photonView.RPC("RpcLand", PhotonTargets.All);
        }

        [PunRPC]
        public void RpcLand()
        {
            OnLand();
        }


        //[Command]
        public void CmdOpen()
        {
            photonView.RPC("RpcOpen", PhotonTargets.All);
        }

        [PunRPC]
        public void RpcOpen()
        {
            OnOpen();
        }


        //[Command]
        public void CmdPosition(Vector3 p)
        {
            photonView.RPC("RpcPosition", PhotonTargets.All, p);
        }

        [PunRPC]
        public void RpcPosition(Vector3 p)
        {
            if (!photonView.isMine)
            {
                transform.position = p;
            }
        }
    }
}