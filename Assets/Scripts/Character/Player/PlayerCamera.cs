using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

namespace FT
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // THE BIGGER IS THIS NUMBER, THE LONGER FOR CAMERA REACH THIS POSITION
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float mininumPivot = -30; // LOWEST ANGLE TO LOOK DOWN
        [SerializeField] float maxinumPivot = 60; // HIGHEST ANGLE TO LOOK UP
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // USED FOR CAMERA COLLISIONS (MOVE THE CAMERA TO THIS POSITION IF COLLIDING)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;    // VALUES USED FOR CAMERA COLLISIONS
        private float targetCameraZPosition;     // VALUES USED FOR CAMERA COLLISIONS

        [Header("Lock On")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float maximumlockOnDistance = 20;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] float unlockedCameraHeight = 1.65f;
        [SerializeField] float lockedCameraHeight = 2.5f;
        [SerializeField] float setCameraHeightSpeed = 1;
        private Coroutine cameraLockOnHeightCoroutine;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player  != null)
            {
                // FOLLOW THE PLAYER
                HandleFollowTarget();
                // ROTATE AROUND THE PLAYER
                HandleRotations();
                // COLLIDE WITH ENVIRO
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;

        }

        private void HandleRotations()
        {
            // IF LOCKED ON, FORCE ROTATION
            if(player.isLockedOn)
            {
                // THIS ROTATES THIS GAME OBJECT
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // THIS ROTATES THE PIVOT OBJECT
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // SAVE OUR ROTATIONS TO OUR LOOK ANGLES, SO WHEN UNLOCK IT DOESNT SNAP TOO FAR AWAY
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // NORMAL ROTATIONS
            else
            {
                // ROTATE LEFT AND RIGHT BASED ON CAMERA INPUTS
                leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                // ROTATE UP AND DOWN CAMERA INPUTS
                upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
                // CLAMP LOOK AND DOWN ROTATE
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, mininumPivot, maxinumPivot);



                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;
                //ROTATES THIS GAME OBJECT IN Y AXIS (LEFT AND RIGHT)
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                // ROTATE THE PIVOT OF THE GAME OBJECT
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }

            
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // CHECK IF HAVE OBJECT IN CAMERA
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // DISTANCE FROM THE COLLISION
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (SNAP BACK)
            if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;


        }

        public void HandleLocationLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity; // WILL BE USE TO DETERMINE THE TARGET CLOSEST TO US
            float shortestDistanceOfLeftTarget = Mathf.Infinity; // WILL BE USED TO DETERMINE SHORTEST DISTANCE ON ONE AXIS TO THE LEFT
            float shortestDistanceOfRightTarget = Mathf.Infinity; // WILL BE USED TO DETERMINE SHORTEST DISTANCE ON ONE AXIS TO THE RIGHT OF CURRENT TARGET

            // USE A LAYERMASK
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if(lockOnTarget != null)
                {
                    // CHECK IF THEY ARE WITHIN OUR FIELD OF VIEW 
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                    // IF TARGET IS DEAD, GO TO NEXT TARGET
                    if(lockOnTarget.characterStatsManager.isDead)
                    {
                        continue;
                    }

                    // IF TARGET IS US, GO TO NEXT TARGET
                    if(lockOnTarget.transform.root == player.transform.root)
                    {
                        continue;
                    }

                    // IF TARGET IS TOO FAR, GO TO NEXT
                    if(distanceFromTarget > maximumlockOnDistance)
                    {
                        continue;
                    }

                    // LASTLY IF THE TARGET IS OUTSIDE FIELD OF VIEW OR IS BLOCKED BY ENVIRO, CHECK NET POTENTIAL TARGET
                    if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // 
                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, 
                            lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, 
                            WorldUtilityManager.instance.GetEnviroLayers()))
                        {
                            // WE HIT SOMETHING, EW CANNOT SEE OUR LOCKON TARGET
                            continue;
                        }
                        else
                        {
                            // OTHERWISE ADD TO POTENTIAL TARGET LIST
                            Debug.Log("Locked On");
                            availableTargets.Add(lockOnTarget);
                        }

                        
                    }

                }
            }
            // SORT TROUGH OUR POTENTIAL TARGETS TO SEE WHICH ONE WE LOCK ONTO FIRST
            for (int k = 0; k < availableTargets.Count; k++) 
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }

                    // IF WE ARE ALREADY LOCKED ON WHEN SEARCHING FOR TARGETS, SEARCH FOR OUR NEAREST LEFT/RIGHT TARGETS
                    if (player.isLockedOn)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] == player.playerCombatManager.currentTarget)
                            continue;

                        // CHECK THE LEFT SIDE FOR TARGETS
                        if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        // CHECK THE RIGHT SIDE FOR TARGETS
                        else if(relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget > shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    player.isLockedOn = false;
                }

            }

        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }
        public void SetPlayerManager(PlayerManager newPlayer)
        {
            player = newPlayer;
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocationLockOnTargets();

            if(nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.isLockedOn = true;
            }

            yield return null;
        }

        public void SetLockCameraHeight()
        {
            if(cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }

            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        private IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while(timer < duration)
            {
                timer += Time.deltaTime;

                if(player != null)
                {
                    if(player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = 
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, 
                            newLockedCameraHeight, ref velocity, setCameraHeightSpeed);


                        cameraPivotTransform.transform.localRotation =
                            Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition =
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }
                yield return null;
            }

            if(player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;

                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition= newUnlockedCameraHeight;
                }
            }

            yield return null;
        }
    }
}