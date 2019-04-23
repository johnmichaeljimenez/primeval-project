using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class Mouselook : PlayerModuleBase
    {

        public Transform bodyTransform;
        public Transform cameraPivotTransform;

        public Transform standPivotTransform, crouchPivotTransform;

        public Vector2 mouseSensitivity;


        public override void Initialize()
        {
            base.Initialize();

            if (playerCharacter.isLocalPlayer)
                CameraManager.instance.SetTarget(cameraPivotTransform);
        }


        void OnDestroy()
        {
            if (!playerCharacter)
                CameraManager.instance.SetTarget(null);
            else if (playerCharacter.isLocalPlayer)
                CameraManager.instance.SetTarget(null);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            cameraPivotTransform.localPosition = playerCharacter.stanceModule.isStanding ? standPivotTransform.localPosition : crouchPivotTransform.localPosition;

            Vector3 v = new Vector3(cameraPivotTransform.localEulerAngles.x, bodyTransform.localEulerAngles.y, 0);

            v.x -= Input.GetAxis("Mouse Y") * mouseSensitivity.y;
            v.y += Input.GetAxis("Mouse X") * mouseSensitivity.x;

            cameraPivotTransform.localEulerAngles = Vector3.right * v.x;
            bodyTransform.localEulerAngles = Vector3.up * v.y;
        }
    }
}