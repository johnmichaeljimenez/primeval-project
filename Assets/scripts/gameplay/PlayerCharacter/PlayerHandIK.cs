using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandIK : MonoBehaviour
{
    public Animator animator;

    public bool enableIK;
    public Transform handLeftTransform;

    float ikWeight;
    void Start()
    {
        ikWeight = 0;
    }

    public void SetIKHand(bool enable, Transform handle)
    {
        enableIK = enable;
        handLeftTransform = handle;
    }

    void OnAnimatorIK()
    {
        if (enableIK)
        {
            if (handLeftTransform != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, handLeftTransform.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, handLeftTransform.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}
