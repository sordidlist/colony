using System.Collections.Generic;
using _Custom.Code.Creature_System.Utilities;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _Custom.Code.Creature_System
{
    public class SensorRaycastHandler : Singleton<SensorRaycastHandler>
    {
        private PopulationHandler populationHandler;
        public Vector3[] sensorOriginPointsArray;
        public RaycastHit[] sensorHitPointsArray;
        public RaycastHit[] closestHitPointsArray;
        private bool debugMode = false;
        public bool launched = false;
        
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
        
        public void SetPopulationHandler(PopulationHandler populationHandler)
        {
            this.populationHandler = populationHandler;
        }

        private LayerMask RayCastLayerMask;

        protected void Awake()
        {
            RayCastLayerMask = ~LayerMask.GetMask("Ignore Raycast", "Player Character", "Creature Agent");
        }

        public void FixedUpdate()
        {
            // Pull aside a batch of creatures upon which to perform sensor raycasting
            List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;
            
            // Define sensor raycast variables
            int creatureCount = batchedCreatureAgents.Count;
            if (debugMode) Debug.Log("Sensor Raycast Handler received batch of " + creatureCount + " creatures.");
            if (debugMode) Debug.Log("Sensor Raycast Handler defining Native memory...");

            NativeArray<RaycastCommand> sensorRaycastCommands = 
                new NativeArray<RaycastCommand>(CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS.Length * creatureCount, 
                    Allocator.Persistent);
            
            NativeArray<Vector3> sensorOriginPoints =
                new NativeArray<Vector3>(creatureCount, Allocator.Persistent);
            NativeArray<RaycastHit> sensorHitPoints =
                new NativeArray<RaycastHit>(sensorRaycastCommands.Length, Allocator.Persistent);
            NativeArray<RaycastHit> closestHitPoints =
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
            if (debugMode) Debug.Log("Sensor Raycast Handler Native memory defined. Executing sensor jobs...");

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

            CreatureSystemJobs.GetNearestRaycastHitsJob getNearestRaycastHitsJob =
                new CreatureSystemJobs.GetNearestRaycastHitsJob
                {
                    closestRaycastHits = closestHitPoints,
                    sensorRaycastHits = sensorHitPoints,
                    directionsToCastSensorRaycastsFromCreatureAgents = directionsToCastSensorRaycastsFromCreatureAgents
                };

            JobHandle getNearestRaycastHitsJobHandle = getNearestRaycastHitsJob.Schedule(creatureTransformAccessArray, sensorRaycastsJobHandle);
            
            getNearestRaycastHitsJobHandle.Complete();
            if (debugMode) Debug.Log("Sensor Raycast Handler sensor jobs complete.");

            sensorOriginPointsArray = sensorOriginPoints.ToArray();
            sensorHitPointsArray = sensorHitPoints.ToArray();
            closestHitPointsArray = closestHitPoints.ToArray();

            if (debugMode)
            {
                foreach (RaycastHit sensorHitPoint in sensorHitPointsArray)
                {
                    Debug.Log("Sensor point: " + sensorHitPoint.point + " detected.");
                }
            }

            // Dispose of native memory
            DisposeOfNativeMemory(sensorRaycastCommands, sensorHitPoints, closestHitPoints, sensorOriginPoints, objectBitePoints, layerMask, 
                directionsToCastSensorRaycastsFromCreatureAgents, maxCreatureAgentScanDistance, creatureTransformAccessArray);

            if (!launched) launched = true;
        }


        private void DisposeOfNativeMemory(NativeArray<RaycastCommand> sensorRaycastCommands, NativeArray<RaycastHit> sensorHitPoints, NativeArray<RaycastHit> closestHitPoints,
            NativeArray<Vector3> sensorOriginPoints, NativeArray<Vector3> objectBitePoints,
            NativeArray<LayerMask> layerMask, NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents, 
            NativeArray<float> maxCreatureAgentScanDistance, TransformAccessArray creatureTransformAccessArray)
        {
            sensorRaycastCommands.Dispose();
            sensorHitPoints.Dispose();
            closestHitPoints.Dispose();
            sensorOriginPoints.Dispose();
            objectBitePoints.Dispose();
            layerMask.Dispose();
            directionsToCastSensorRaycastsFromCreatureAgents.Dispose();
            maxCreatureAgentScanDistance.Dispose();
            creatureTransformAccessArray.Dispose();
        }
    }
}