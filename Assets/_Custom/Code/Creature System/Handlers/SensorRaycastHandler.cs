using System.Collections.Generic;
using _Custom.Code.Creature_System.Utilities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _Custom.Code.Creature_System
{
    public class SensorRaycastHandler : Singleton<SensorRaycastHandler>
    {
        private PopulationHandler populationHandler;
        
        public void SetPopulationHandler(PopulationHandler populationHandler)
        {
            this.populationHandler = populationHandler;
        }
        
        public bool HasNearbySurfacePoint()
        {

            return false;
        }

        public void GetNearbySurfacePoint()
        {
            
        }
        
        
        private LayerMask RayCastLayerMask;

        protected void Awake()
        {
            RayCastLayerMask = ~LayerMask.GetMask("Ignore Raycast", "Player Character");
        }

        public void Update()
        {
            // Pull aside a batch of creatures upon which to perform sensor raycasting
            List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;
            
            // Define sensor raycast variables
            int creatureCount = batchedCreatureAgents.Count;
            
            NativeArray<RaycastCommand> sensorRaycastCommands = 
                new NativeArray<RaycastCommand>(CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS.Length * creatureCount, 
                    Allocator.Persistent);
            
            NativeArray<Vector3> sensorOriginPoints =
                new NativeArray<Vector3>(creatureCount, Allocator.Persistent);
            NativeArray<RaycastHit> sensorHitPoints =
                new NativeArray<RaycastHit>(sensorRaycastCommands.Length, Allocator.Persistent);
            NativeArray<Vector3> objectBitePoints =
                new NativeArray<Vector3>(creatureCount, Allocator.Persistent);
            NativeArray<LayerMask> layerMask = new NativeArray<LayerMask>(1, Allocator.Persistent);
            NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents =
                new NativeArray<Vector3>(
                    CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS.Length,
                    Allocator.Persistent);
            NativeArray<float> maxCreatureAgentScanDistance = new NativeArray<float>(1, Allocator.Persistent);
            
            layerMask[0] = RayCastLayerMask;
            for (int directionIndex = 0;
                directionIndex < CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS.Length;
                directionIndex++)
            {
                directionsToCastSensorRaycastsFromCreatureAgents[directionIndex] =
                    CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS[directionIndex];
            }
            maxCreatureAgentScanDistance[0] = CreatureAgentConfig.MAX_CREATURE_AGENT_SCAN_DISTANCE;
            
            List<Transform> creatureTransformsList = new List<Transform>();
            for (var i = 0; i < creatureCount; i++)
                creatureTransformsList.Add(populationHandler.batchedCreatureAgents[i].transform);

            Transform[] creatureTransformsArray = creatureTransformsList.ToArray();
            TransformAccessArray creatureTransformAccessArray =
                new TransformAccessArray(creatureTransformsArray);

            // Execute sensor raycast and store results
            CreatureSystemJobs.GetRaycastDirectionsRelativeToCreatureAgentsTransformsJob
                getRaycastDirectionsRelativeToCreatureAgentTransformsJob =
                    new CreatureSystemJobs.GetRaycastDirectionsRelativeToCreatureAgentsTransformsJob{
                        sensorRaycastCommands = sensorRaycastCommands,
                        sensorOriginPoints = sensorOriginPoints,
                        objectBitePoints = objectBitePoints,
                        layerMask = layerMask,
                        directionsToCastSensorRaycastsFromCreatureAgents = directionsToCastSensorRaycastsFromCreatureAgents,
                        maxCreatureAgentScanDistance = maxCreatureAgentScanDistance
                    };
            JobHandle getDirectionsJobHandle =
                getRaycastDirectionsRelativeToCreatureAgentTransformsJob.Schedule(creatureTransformAccessArray);
            
            JobHandle.ScheduleBatchedJobs();
            
            // Fire all raycasts and store results
            JobHandle sensorRaycastsJobHandle = RaycastCommand.ScheduleBatch(sensorRaycastCommands, sensorHitPoints,
                CreatureAgentConfig.SENSOR_RAYCAST_PARALLEL_JOBS_COUNT, getDirectionsJobHandle);
            
            sensorRaycastsJobHandle.Complete();

            // Dispose of native memory
            DisposeOfNativeMemory(sensorRaycastCommands, sensorHitPoints, sensorOriginPoints, objectBitePoints, layerMask, 
                directionsToCastSensorRaycastsFromCreatureAgents, maxCreatureAgentScanDistance, creatureTransformAccessArray);
        }


        private void DisposeOfNativeMemory(NativeArray<RaycastCommand> sensorRaycastCommands, NativeArray<RaycastHit> sensorHitPoints,
            NativeArray<Vector3> sensorOriginPoints, NativeArray<Vector3> objectBitePoints,
            NativeArray<LayerMask> layerMask, NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents, 
            NativeArray<float> maxCreatureAgentScanDistance, TransformAccessArray creatureTransformAccessArray)
        {
            sensorRaycastCommands.Dispose();
            sensorHitPoints.Dispose();
            sensorOriginPoints.Dispose();
            objectBitePoints.Dispose();
            layerMask.Dispose();
            directionsToCastSensorRaycastsFromCreatureAgents.Dispose();
            maxCreatureAgentScanDistance.Dispose();
            creatureTransformAccessArray.Dispose();
        }

        public void OldUpdate()
        {

                    /*var getRaycastDirectionsJobHandle =
                        getRaycastDirectionsJob.Schedule(creatureTransformAccessArray);*/
                    JobHandle.ScheduleBatchedJobs();

                    /*
                     * FIRE THE RAYCASTS
                     */

                    /*var forwardRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(forwardRaycastCommands, forwardRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var leftRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(leftRaycastCommands, leftRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var rightRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(rightRaycastCommands, rightRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var upRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(upRaycastCommands, upRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var downRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(downRaycastCommands, downRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var downBackRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(downBackRacycastCommands, downBackRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var forwardDownRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(forwardDownRaycastCommands, forwardDownRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var forwardDownLeftRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(forwardDownLeftRaycastCommands, forwardDownLeftRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var forwardDownRightRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(forwardDownRightRaycastCommands, forwardDownRightRaycastHits, 1,
                            getRaycastDirectionsJobHandle);
                    var objectBiteNormalsRaycastJobHandle =
                        RaycastCommand.ScheduleBatch(biteObjectNormalRaycastCommands, biteObjectNormalRaycastHits, 1,
                            getRaycastDirectionsJobHandle);*/

                    /*forwardRaycastJobHandle.Complete();
                    leftRaycastJobHandle.Complete();
                    rightRaycastJobHandle.Complete();
                    upRaycastJobHandle.Complete();
                    downRaycastJobHandle.Complete();
                    downBackRaycastJobHandle.Complete();
                    forwardDownRaycastJobHandle.Complete();
                    forwardDownLeftRaycastJobHandle.Complete();
                    forwardDownRightRaycastJobHandle.Complete();
                    objectBiteNormalsRaycastJobHandle.Complete();*/

                    //for (var i = 0; i < agentIndeces.Count; i++)
                    //{
                        /*var creatureAgent = creaturePopulationController.creatureAgents[agentIndeces[i]];
                        creatureAgent.forwardRaycastHit = forwardRaycastHits[i];
                        creatureAgent.upRaycastHit = upRaycastHits[i];
                        creatureAgent.downRaycastHit = downRaycastHits[i];
                        creatureAgent.downBackRaycastHit = downBackRaycastHits[i];
                        creatureAgent.leftRaycastHit = leftRaycastHits[i];
                        creatureAgent.rightRaycastHit = rightRaycastHits[i];
                        creatureAgent.forwardDownRaycastHit = forwardDownRaycastHits[i];
                        creatureAgent.forwardDownLeftRaycastHit = forwardDownLeftRaycastHits[i];
                        creatureAgent.forwardDownRightRaycastHit = forwardDownRightRaycastHits[i];

                        creatureAgent.closestRaycastHit = downRaycastHits[i];

                        CompareRaycastHitToCurrentClosest(forwardRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(upRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(downRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(downBackRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(leftRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(rightRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(forwardDownRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(forwardDownLeftRaycastHits[i], creatureAgent);
                        CompareRaycastHitToCurrentClosest(forwardDownRightRaycastHits[i], creatureAgent);

                        try
                        {
                            if (biteObjectNormalRaycastHits[i].collider.gameObject.Equals(creatureAgent.biteObject))
                                creatureAgent.objectBiteNormal = biteObjectNormalRaycastHits[i].normal;
                        }
                        catch (NullReferenceException e)
                        {
                            // Debug.LogError(e);
                        }

                        creatureAgent.raycastResultsAreReady = true;
                        //Debug.Log(creatureAgent.ToString());*//*
                    }

                    creatureTransformAccessArray.Dispose();
                    forwardRaycastCommands.Dispose();
                    leftRaycastCommands.Dispose();
                    rightRaycastCommands.Dispose();
                    upRaycastCommands.Dispose();
                    downRaycastCommands.Dispose();
                    downBackRacycastCommands.Dispose();
                    forwardDownRaycastCommands.Dispose();
                    forwardDownLeftRaycastCommands.Dispose();
                    forwardDownRightRaycastCommands.Dispose();
                    biteObjectNormalRaycastCommands.Dispose();

                    forwardRaycastHits.Dispose();
                    leftRaycastHits.Dispose();
                    rightRaycastHits.Dispose();
                    upRaycastHits.Dispose();
                    downRaycastHits.Dispose();
                    downBackRaycastHits.Dispose();
                    forwardDownRaycastHits.Dispose();
                    forwardDownRightRaycastHits.Dispose();
                    forwardDownLeftRaycastHits.Dispose();
                    biteObjectNormalRaycastHits.Dispose();

                    sensorOriginPoints.Dispose();
                    layerMask.Dispose();
                    objectBitePoints.Dispose();
                }
                catch (MissingReferenceException e)
                {
                    Debug.LogWarning(e);
                }*/
        }

        /*private void CompareRaycastHitToCurrentClosest(RaycastHit raycastHit, CreatureAgent.CreatureAgent creatureAgent)
        {
            //if (raycastHit.distance != 0)
                //if (!raycastHit.collider.gameObject.Equals(gameObject))
                    //if (creatureAgent.closestRaycastHit.distance > raycastHit.distance)
                    //    creatureAgent.closestRaycastHit = raycastHit;
        }*/
    }
}