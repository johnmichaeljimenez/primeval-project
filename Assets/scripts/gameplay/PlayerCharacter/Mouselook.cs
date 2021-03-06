﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class Mouselook : PlayerModuleBase
    {

        public Transform bodyTransform;
        public Transform cameraPivotTransform;

        public Transform standPivotTransform, crouchPivotTransform, fpsPivotTransform;

        public Vector2 mouseSensitivity;

        public Vector2 normalizedAngle { get; private set; }

        public bool canControl;


        public override void Initialize()
        {
            base.Initialize();

            if (playerCharacter.photonView.isMine)
                CameraManager.instance.SetTarget(cameraPivotTransform);

            normalizedAngle = Vector2.zero;
        }


        void OnDestroy()
        {
            if (!playerCharacter)
                CameraManager.instance.SetTarget(null);
            else if (playerCharacter.photonView.isMine)
                CameraManager.instance.SetTarget(null);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Vector3 target = standPivotTransform.position;

            if (playerCharacter.vitalityModule.isDead)
            {
                cameraPivotTransform.position = fpsPivotTransform.position;
                cameraPivotTransform.rotation = fpsPivotTransform.rotation;
            }
            else
            {
                if (!canControl)
                {
                    cameraPivotTransform.localPosition = standPivotTransform.localPosition;
                    cameraPivotTransform.localRotation = Quaternion.identity;
                    return;
                }

                if (playerCharacter.stanceModule.isStanding)
                {
                    target = standPivotTransform.localPosition;
                }
                else
                {
                    target = crouchPivotTransform.localPosition;
                }

                Vector3 stanceTarget = Vector3.Lerp(cameraPivotTransform.localPosition, target, Time.deltaTime * 10);
                cameraPivotTransform.localPosition = stanceTarget;

                Vector3 v = new Vector3(cameraPivotTransform.localEulerAngles.x, bodyTransform.localEulerAngles.y, 0);

                v.x -= Input.GetAxis("Mouse Y") * mouseSensitivity.y;
                v.y += Input.GetAxis("Mouse X") * mouseSensitivity.x;
                v.x = ClampAngle(v.x, -80, 80);

                cameraPivotTransform.localEulerAngles = Vector3.right * v.x;
                bodyTransform.localEulerAngles = Vector3.up * v.y;

                normalizedAngle = Quaternion.Euler(cameraPivotTransform.eulerAngles) * Vector3.forward;
            }
        }

        float ClampAngle(float a, float min, float max)
        {
            while (max < min) max += 360;
            while (a > max) a -= 360;
            while (a < min) a += 360;

            if (a > max)
            {
                if (a - (max + min) * 0.5f < 180)
                {
                    return max;
                }
                else
                {
                    return min;
                }
            }
            else
            {
                return a;
            }
        }
    }


}