using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.PlayerCharacter
{
    public class Movement : PlayerModuleBase
    {

        public CharacterController characterController;
        public SpeedModifier baseMovementSpeed;
        public SpeedModifier runMultiplier;
        public SpeedModifier crouchMultiplier;
        public float gravity, jumpForce, groundMargin;

        Vector3 sphereCastOrigin
        {
            get
            {
                return playerCharacter.transform.position + (Vector3.up / 2);
            }
        }
        float sphereCastRadius = 0.5f;

        [HideInInspector]
        public bool isRunning;

        [HideInInspector]
        public bool isGrounded;
        [HideInInspector]
        float hitDistance;
        float hitDistance_ceiling;
        float previousYPosition;
        float sourceHeight;
        bool getSourceHeight;

        public float runDelay { get; private set; }

        RaycastHit hitInfo_floor;
        RaycastHit hitInfo_ceiling;

        Vector3 movementDirection;
        [HideInInspector]
        public Vector3 inputDirection;

        public LayerMask collisionMask;

        public AudioClip jumpClip, landClip;

        public override void Initialize()
        {
            previousYPosition = characterController.transform.position.y;
            sourceHeight = previousYPosition;
            getSourceHeight = true;

            base.Initialize();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckGrounded();
            Move();
        }

        void Move()
        {
            float y = movementDirection.y;
            float yy = characterController.transform.position.y;

            if (isGrounded)
            {
                inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                movementDirection = inputDirection;
                isRunning = Input.GetKey(KeyCode.LeftShift) && playerCharacter.stanceModule.isStanding;// && movementDirection.z > 0;

                Vector3 speed = new Vector3(baseMovementSpeed.side, 0, movementDirection.z > 0 ? baseMovementSpeed.forward : baseMovementSpeed.back);
                if (playerCharacter.stanceModule.isCrouching)
                {
                    runDelay = 0;
                    speed.Scale(new Vector3(crouchMultiplier.side, 0, movementDirection.z > 0 ? crouchMultiplier.forward : crouchMultiplier.back));
                }
                else if (isRunning)
                {
                    runDelay = Mathf.Min(runDelay + Time.deltaTime, 1);
                    Vector3 runScale = new Vector3(runMultiplier.side, 0, movementDirection.z > 0 ? runMultiplier.forward : runMultiplier.back);

                    speed.Scale(Vector3.Lerp(Vector3.one, runScale, runDelay));
                }
                else
                {
                    runDelay = 0;
                }
                movementDirection.Scale(speed);
                movementDirection = playerCharacter.transform.TransformDirection(movementDirection);

                if (Input.GetKeyDown(KeyCode.Space) && playerCharacter.stanceModule.isStanding)
                {
                    y = jumpForce;
                    playerCharacter.audioPlayerModule.PlaySound(jumpClip, true);
                }
                else
                {
                    y = 0;
                }
            }
            else
            {
                runDelay = 0;
                inputDirection = Vector2.zero;
                y -= Time.deltaTime * gravity;

                if (y > 0)
                {
                    Physics.SphereCast(sphereCastOrigin, sphereCastRadius, Vector3.up, out hitInfo_ceiling, 300, collisionMask);
                    hitDistance_ceiling = hitInfo_ceiling.distance;
                    if (hitDistance_ceiling > 0 && hitDistance_ceiling < 1.1f)
                    {
                        y = 0; //hit ceiling

                        sourceHeight = characterController.transform.position.y;
                        getSourceHeight = false;
                    }
                }


                if (getSourceHeight)
                {
                    if (Mathf.Abs(y) <= 0.05f)
                    {
                        getSourceHeight = true;
                        sourceHeight = characterController.transform.position.y;
                    }
                }
            }

            movementDirection.y = y;

            characterController.Move(movementDirection * Time.deltaTime);
        }

        void CheckGrounded()
        {
            Physics.SphereCast(sphereCastOrigin, sphereCastRadius, Vector3.down, out hitInfo_floor, 300, collisionMask);
            hitDistance = hitInfo_floor.distance;

            bool pGrounded = isGrounded;
            isGrounded = hitDistance <= groundMargin;
            getSourceHeight = true;
            if (!pGrounded && isGrounded)
            {
                previousYPosition = characterController.transform.position.y - sourceHeight;
                // print("y: " + Mathf.RoundToInt(previousYPosition));

                if (previousYPosition < 0)
                {
                    if (previousYPosition >= -4)
                    {
                        CameraAnimator.instance.LandAnimation();
                    }
                    else if (previousYPosition >= -6)
                    {
                        playerCharacter.audioPlayerModule.PlaySound(landClip,true);
                        CameraAnimator.instance.HardLandAnimation();
                    }
                }

                sourceHeight = characterController.transform.position.y;
            }
            //canJump = isGrounded && hitInfo.normal.
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(sphereCastOrigin + (Vector3.down * hitDistance), sphereCastRadius);
        }

        [System.Serializable]
        public struct SpeedModifier
        {
            public float forward, side, back;
        }
    }

}