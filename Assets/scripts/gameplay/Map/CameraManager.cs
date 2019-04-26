using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Primeval.Item;

public class CameraManager : GenericSingletonClass<CameraManager>
{

    public Camera mainCamera;
    public Transform itemFPSContainer;

    public float normalFov, runningFov, aimingFov;
    float targetFov;
    float currentFov;

    Transform target;

    public override void Awake()
    {
        base.Awake();

        targetFov = normalFov;
        currentFov = targetFov;
        mainCamera.fieldOfView = currentFov;
    }

    void Update()
    {
        if (target)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }

        FieldOfView();
    }

    void FieldOfView()
    {
        if (!PlayerCharacter.myPlayer)
            return;

        PlayerCharacter p = PlayerCharacter.myPlayer;
        if (Weapon.aiming)
        {
            targetFov = aimingFov;
        }else{
            if (p.movementModule.isRunning && p.movementModule.inputDirection.z > 0)
                targetFov = Mathf.Lerp(normalFov, runningFov, p.movementModule.runDelay);
            else
                targetFov = normalFov;
        }

        currentFov = Mathf.Lerp(currentFov, targetFov, Time.deltaTime*10);
        mainCamera.fieldOfView = currentFov;
    }

    public void SetTarget(Transform t)
    {
        if (t)
        {
            target = t;
            // transform.localPosition = Vector3.zero;
            // transform.localRotation = Quaternion.identity;

            transform.position = t.position;
            transform.rotation = t.rotation;

            if (PlayerCharacter.myPlayer)
                CameraAnimator.instance.Initialize();
        }
        else
        {
            targetFov = normalFov;
        }
    }

    public bool IsTarget(Transform t)
    {
        return target == t;
    }

}
