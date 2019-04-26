using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Primeval.Item;

public class CameraAnimator : GenericSingletonClass<CameraAnimator>
{
    public static bool interrupt;
    public Animator animator;

    PlayerCharacter player;

    float t;
    bool initialized;
    public override void Initialize()
    {
        base.Initialize();
        initialized = true;
        animator = GetComponent<Animator>();
        player = PlayerCharacter.myPlayer;
        SetState(CameraStates.Idle);
        t = 0;
    }

    void Update()
    {
        if (!initialized)
            return;

        if (!player)
        {
            SetState(CameraStates.Idle);
            return;
        }

        bool isRunning, isWalking;
        isRunning = player.movementModule.isRunning && player.movementModule.inputDirection.sqrMagnitude > 0 && player.movementModule.runDelay > 0f && player.movementModule.isGrounded;
        isWalking = player.movementModule.isGrounded && player.movementModule.inputDirection.sqrMagnitude > 0;

        t = Mathf.Lerp(t, isRunning? 1 : isWalking? 0.5f : 0, Time.deltaTime*10);
        animator.SetFloat("movement", t);
    }

    public void LandAnimation()
    {
        SetState(CameraStates.Land);
    }
    public void HardLandAnimation()
    {
        SetState(CameraStates.LandHard);
    }

    public void OnLandStart()
    {
        interrupt = true;
    }
    
    public void OnLandEnd()
    {
        interrupt = false;
        SetState(CameraStates.Idle);
    }

    public void SetState(CameraStates state)
    {
        animator.SetInteger("state", (int)state);
    }


    public enum CameraStates
    {
        Idle,
        Walk,
        Run,
        Land,
        LandHard,
        Crouch,
        Stand
    }
}
