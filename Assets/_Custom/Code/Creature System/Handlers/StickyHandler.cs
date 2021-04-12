using System;
using System.Collections.Generic;
using _Custom.Code.Creature_System.Utilities;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class StickyHandler : Singleton<StickyHandler>
    {
        private PopulationHandler populationHandler;
        private SensorRaycastHandler sensorRaycastHandler;
        private bool debugMode = false;
        
        [Button("Toggle Debug Mode")]
        [GUIColor("GetToggleColor")]
        private void ToggleDebugMode()
        {
            debugMode = !debugMode;
        }

        private Color GetToggleColor()
        {
            if (debugMode) return new Color(0, 0.5f, 0, 0.5f);
            return new Color(0.5f, 0f, 0f, 0.5f);
        }
        
        public void CheckStickiness()
        {
            if (debugMode) {Debug.Log("Checking creature agent stickiness...");}
            List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;
            RaycastHit[] closestHitPointsArray = sensorRaycastHandler.closestHitPointsArray;

            NativeArray<RaycastHit> closestHitPoints = new NativeArray<RaycastHit>(closestHitPointsArray.Length, Allocator.Persistent);
            NativeArray<float> maxStickyDistance =
                new NativeArray<float>(batchedCreatureAgents.Count, Allocator.Persistent);
            NativeArray<bool> isSticking = new NativeArray<bool>(batchedCreatureAgents.Count, Allocator.Persistent);
            NativeArray<bool> isFalling = new NativeArray<bool>(batchedCreatureAgents.Count, Allocator.Persistent);
            NativeArray<int> creatureAgentLayer = new NativeArray<int>(1, Allocator.Persistent);

            creatureAgentLayer[0] = LayerMask.NameToLayer("Creature Agent");
            for (int creatureAgentIndex = 0; creatureAgentIndex < batchedCreatureAgents.Count; creatureAgentIndex++)
            {
                if (debugMode)
                {
                    Debug.Log("Defining hit point and sticky variables for creature agent: " + batchedCreatureAgents[creatureAgentIndex].name);
                }
                closestHitPoints[creatureAgentIndex] = closestHitPointsArray[creatureAgentIndex];
                maxStickyDistance[creatureAgentIndex] =
                    batchedCreatureAgents[creatureAgentIndex].GetMaximumStickyDistance();
                
                CreatureAgent.CreatureAgent batchedCreatureAgent = batchedCreatureAgents[creatureAgentIndex];
                batchedCreatureAgent.SetClosestRaycastHit(closestHitPointsArray[creatureAgentIndex]);
                //try
                //{
                //isFalling[creatureAgentIndex] = closestHitPointsArray[creatureAgentIndex].collider.gameObject.layer
                //    .Equals(creatureAgentLayer[0]);
                isFalling[creatureAgentIndex] = true;
                if (debugMode)
                {
                    Debug.Log("Set isFalling: " + isFalling[creatureAgentIndex]);
                }
                //}
                //catch (NullReferenceException e)
                //{
                //    isFalling[creatureAgentIndex] = true;
                //}
            }

            
            if (debugMode) {Debug.Log("Sticky check variables defined.");}

            JobHandle checkCreatureAgentStickinessJob =
                new CreatureSystemJobs.CheckCreatureAgentStickinessJob
                {
                    closestHitPoints = closestHitPoints,
                    maxStickyDistance = maxStickyDistance,
                    isSticking = isSticking,
                    isFalling = isFalling,
                    creatureAgentLayer = creatureAgentLayer
                }.Schedule(batchedCreatureAgents.Count, 64);

            checkCreatureAgentStickinessJob.Complete();

            if (debugMode) {Debug.Log("Stickiness job completed.");}

            for (int creatureAgentIndex = 0; creatureAgentIndex < batchedCreatureAgents.Count; creatureAgentIndex++)
            {
                batchedCreatureAgents[creatureAgentIndex].SetIsSticking(isSticking[creatureAgentIndex]);
                batchedCreatureAgents[creatureAgentIndex].SetIsFalling(isFalling[creatureAgentIndex]);
                if (debugMode) {Debug.Log("Creature agent " + batchedCreatureAgents[creatureAgentIndex].name + 
                                          " stickiness is set to : " + batchedCreatureAgents[creatureAgentIndex].IsSticking());}
                if (debugMode) {Debug.Log("Creature agent " + batchedCreatureAgents[creatureAgentIndex].name + 
                                          " closest raycast hit is " + sensorRaycastHandler.closestHitPointsArray[creatureAgentIndex]);}
            }
            
            if (debugMode) {Debug.Log("Creature agent stickiness values set.");}

            closestHitPoints.Dispose();
            maxStickyDistance.Dispose();
            isSticking.Dispose();
            isFalling.Dispose();
            creatureAgentLayer.Dispose();
        }

        public void SetHandlers(PopulationHandler populationHandler, SensorRaycastHandler sensorRaycastHandler)
        {
            this.populationHandler = populationHandler;
            this.sensorRaycastHandler = sensorRaycastHandler;
        }

        private void FixedUpdate()
        {
            if (sensorRaycastHandler.launched)
            {
                try
                {
                    CheckStickiness();
                    List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;
                    for (int creatureAgentIndex = 0;
                        creatureAgentIndex < batchedCreatureAgents.Count;
                        creatureAgentIndex++)
                    {
                        if (debugMode)
                        {
                            Debug.Log("Magnetic rotating creature agent: " +
                                      batchedCreatureAgents[creatureAgentIndex].name);
                        }
                        MagneticRotate(batchedCreatureAgents[creatureAgentIndex]);

                        if (debugMode)
                        {
                            Debug.Log("Gravitating creature agent: " + batchedCreatureAgents[creatureAgentIndex].name);
                        }
                        Gravitate(batchedCreatureAgents[creatureAgentIndex]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    if (debugMode) 
                        Debug.LogWarning(e);
                }
            } else if (debugMode)
            {
                Debug.Log("StickyHandler's reference to SensorRaycastHandler is null.");
            }
        }

        private void MagneticRotate(CreatureAgent.CreatureAgent creatureAgent)
        {
            Quaternion stickedRotation = GetMagneticRotation(creatureAgent);
            Quaternion ignoreQuaternion = new Quaternion(0f, 0f, 0f, 1f);
            if (!stickedRotation.Equals(ignoreQuaternion))
                creatureAgent.GetRigidbody().transform.rotation = Quaternion.Slerp(
                    creatureAgent.GetRigidbody().transform.rotation, stickedRotation,
                    Time.fixedDeltaTime * CreatureAgentConfig.DEFAULT_MAGNETIC_ROTATION_SPEED_GROUNDED);
        }
        
        private Quaternion GetMagneticRotation(CreatureAgent.CreatureAgent creatureAgent)
        {
            Vector3 closestSurfaceNormal = creatureAgent.GetClosestRaycastHit().normal;
            Vector3 lookDirection = Vector3.Cross(creatureAgent.GetRigidbody().transform.right, closestSurfaceNormal);
            if (closestSurfaceNormal == Vector3.zero || lookDirection == Vector3.zero) return Quaternion.identity;

            return Quaternion.LookRotation(lookDirection, closestSurfaceNormal);
        }

        private void Gravitate(CreatureAgent.CreatureAgent creatureAgent)
        {
            Transform creatureTransform = creatureAgent.GetRigidbody().transform;
            if (creatureAgent.IsSticking() && !creatureAgent.IsFalling())
            {
                Vector3 gravitateForceVector = -creatureTransform.up *
                                               (Time.deltaTime * CreatureAgentConfig.STICKING_GRAVITY_SPEED);
                creatureAgent.GetRigidbody().AddForce(gravitateForceVector);
            }
            else if (creatureAgent.IsSticking() && creatureAgent.IsFalling())
            {
                Vector3 gravitateForceVector = (-creatureTransform.up *
                                               (Time.deltaTime * CreatureAgentConfig.STICKING_GRAVITY_SPEED)) + 
                                               (-Vector3.up * (Time.deltaTime * CreatureAgentConfig.DEFAULT_GRAVITY_SPEED));
                creatureAgent.GetRigidbody().AddForce(gravitateForceVector);
            }
            else
            {
                Vector3 gravitateForceVector = -Vector3.up * (Time.deltaTime * CreatureAgentConfig.DEFAULT_GRAVITY_SPEED);
                creatureAgent.GetRigidbody()
                    .AddForce(gravitateForceVector);
            }
        }
    }
}