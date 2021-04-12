using System;
using _Custom.Code.Creature_System.Utilities;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace _Custom.Code.Creature_System.Debug_System
{
    public class DebugHandler : Singleton<DebugHandler>
    {
        private SensorRaycastHandler sensorRaycastHandler;
        private PopulationHandler populationHandler;
        private bool executeDebugHandler;
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

        [Button("Toggle Debug Handler")]
        [GUIColor("GetActivationColor")]
        public void ToggleDebugHandler()
        {
            executeDebugHandler = !executeDebugHandler;
        }

        private Color GetActivationColor()
        {
            if (executeDebugHandler) return new Color(0, 0.5f, 0, 0.5f);
            return new Color(0.5f, 0f, 0f, 0.5f);
        }

        public void SetHandlers(SensorRaycastHandler sensorRaycastHandler, PopulationHandler populationHandler)
        {
            this.sensorRaycastHandler = sensorRaycastHandler;
            this.populationHandler = populationHandler;
        }

        public void Start()
        {
            executeDebugHandler = DebugSystemConfig.START_DEBUG_HANDLER;
        }

        private void Update()
        {
            if (executeDebugHandler && sensorRaycastHandler.sensorHitPointsArray != null)
            {
                // Establish drawing variables

                if (debugMode) Debug.Log("Debug Handler establishing drawing variables...");

                CommandBuilder builder = DrawingManager.GetBuilder(true);
                builder.Preallocate(DebugSystemConfig.LINE_DRAWER_COMMAND_BUILDER_ALLOCATE_SIZE *
                                    DebugSystemConfig.LINE_DRAWER_COMMAND_BUILDER_ALLOCATE_SIZE *
                                    DebugSystemConfig.LINE_DRAWER_COMMAND_BUILDER_ALLOCATE_SIZE * 4);
                NativeArray<Vector3> scanDirections = new NativeArray<Vector3>(
                    CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS.Length,
                    Allocator.Persistent);
                for (int scanDirectionIndex = 0;
                    scanDirectionIndex < CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS
                        .Length;
                    scanDirectionIndex++)
                {
                    if (debugMode)
                        Debug.Log("Debug Handler establishing direction: " +
                                  CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS[
                                      scanDirectionIndex] + "...");
                    scanDirections[scanDirectionIndex] =
                        CreatureAgentConfig.DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS[scanDirectionIndex];
                }

                NativeArray<Color> lineColors = new NativeArray<Color>(scanDirections.Length, Allocator.Persistent);
                if (debugMode) Debug.Log("Debug Handler establishing line colors...");

                for (int lineColorIndex = 0; lineColorIndex < lineColors.Length; lineColorIndex++)
                {
                    float colorValue = (lineColorIndex / (float) lineColors.Length);
                    lineColors[lineColorIndex] = new Color(colorValue, colorValue, colorValue);
                }

                int creatureAgentCount = populationHandler.GetCurrentCreatureAgentBatchSize();
                if (debugMode)
                    Debug.Log("Debug Handler received batch of " + creatureAgentCount +
                              " creatures to draw lines for.");

                NativeArray<Vector3> creatureLinesOriginPoints =
                    new NativeArray<Vector3>(creatureAgentCount, Allocator.Persistent);
                NativeArray<RaycastHit> raycastHitPoints = new NativeArray<RaycastHit>(
                    sensorRaycastHandler.sensorHitPointsArray.Length,
                    Allocator.Persistent);
                NativeArray<float> hitPointSphereRadius = new NativeArray<float>(1, Allocator.Persistent);
                NativeArray<RaycastHit> closestHitPoints =
                    new NativeArray<RaycastHit>(creatureAgentCount, Allocator.Persistent);
                NativeArray<Vector3> objectBitePoints =
                    new NativeArray<Vector3>(creatureAgentCount, Allocator.Persistent);
                NativeArray<Vector3> objectBiteNormals =
                    new NativeArray<Vector3>(creatureAgentCount, Allocator.Persistent);
                NativeArray<int> isSticking = new NativeArray<int>(creatureAgentCount, Allocator.Persistent);
                NativeArray<int> pheromoneDetection = new NativeArray<int>(creatureAgentCount, Allocator.Persistent);

                for (int creatureAgentIndex = 0;
                    creatureAgentIndex < creatureAgentCount;
                    creatureAgentIndex++)
                {
                    try
                    {
                        if (debugMode)
                            Debug.Log("Debug Handler drawing lines for creature agent: " + creatureAgentIndex + "...");
                        CreatureAgent.CreatureAgent creatureAgent =
                            populationHandler.batchedCreatureAgents[creatureAgentIndex];

                        Vector3 creatureLinesOriginPoint = creatureAgent.gameObject.transform.position;
                        if (debugMode)
                            Debug.Log("Debug Handler creature agent: " + creatureAgentIndex + " origin point: " +
                                      creatureLinesOriginPoint);
                        if (debugMode)
                            Debug.Log("Origin Points is created: " + creatureLinesOriginPoints.IsCreated +
                                      ", capacity: " + creatureLinesOriginPoints.Length);

                        creatureLinesOriginPoints[creatureAgentIndex] = creatureLinesOriginPoint;

                        for (int scanDirectionIndex = 0;
                            scanDirectionIndex < scanDirections.Length;
                            scanDirectionIndex++)
                        {
                            raycastHitPoints[creatureAgentIndex * scanDirections.Length + scanDirectionIndex] =
                                sensorRaycastHandler.sensorHitPointsArray[
                                    creatureAgentIndex * scanDirections.Length + scanDirectionIndex];
                            if (debugMode)
                                Debug.Log("Debug Handler raycastHitPoint[" + creatureAgentIndex *
                                    scanDirections.Length + scanDirectionIndex + "] = " + raycastHitPoints[
                                        creatureAgentIndex *
                                        scanDirections.Length + scanDirectionIndex]);
                        }

                        closestHitPoints[creatureAgentIndex] =
                            sensorRaycastHandler.closestHitPointsArray[creatureAgentIndex];
                        if (debugMode)
                            Debug.Log("Debug Handler closestHitPoints[" + creatureAgentIndex + "] = " +
                                      closestHitPoints[creatureAgentIndex]);

                        if (creatureAgent.GetDetectsFood() > creatureAgent.GetDetectsThreat())
                        {
                            pheromoneDetection[creatureAgentIndex] = 1;
                        } 
                        else if (creatureAgent.GetDetectsThreat() > creatureAgent.GetDetectsFood())
                        {
                            pheromoneDetection[creatureAgentIndex] = 2;
                        } 
                        else if (creatureAgent.GetDetectsFood() == 0 && creatureAgent.GetDetectsThreat() == 0)
                        {
                            pheromoneDetection[creatureAgentIndex] = 0;
                        }
                        else
                        {
                            pheromoneDetection[creatureAgentIndex] = 3;
                        }

                        //objectBitePoints[creatureAgentIndex] = creatureAgent.objectBitePoint;
                        //objectBiteNormals[creatureAgentIndex] = creatureAgent.objectBiteNormal;

                        hitPointSphereRadius[0] = DebugSystemConfig.DEBUG_RAYCASTHIT_SPHERE_COLLIDER_RADIUS;
                        if (creatureAgent.IsSticking() && creatureAgent.IsFalling())
                        {
                            isSticking[creatureAgentIndex] = 2;
                        } else if (creatureAgent.IsSticking())
                        {
                            isSticking[creatureAgentIndex] = 0;
                        }
                        else
                        {
                            isSticking[creatureAgentIndex] = 1;
                        }
                        
                        if (debugMode)
                            Debug.Log("Debug Handler closestHitPoints[" + creatureAgentIndex + "] = " +
                                      closestHitPoints[creatureAgentIndex]);
                    }
                    catch (MissingReferenceException e)
                    {
                        Debug.LogError(e);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        if (debugMode)
                            Debug.LogWarning(e);
                    }

                    var drawingJob = new CreatureSystemJobs.DrawingJob
                    {
                        builder = builder,
                        origin = creatureLinesOriginPoints,
                        sphereRadius = hitPointSphereRadius,
                        directionsToScan = scanDirections,
                        lineColors = lineColors,
                        raycastHitPoints = raycastHitPoints,
                        closestHitPoints = closestHitPoints,
                        objectBitePoints = objectBitePoints,
                        objectBiteNormals = objectBiteNormals,
                        isSticking = isSticking,
                        pheromoneDetection = pheromoneDetection
                    };
                    drawingJob.Schedule().Complete();
                }

                lineColors.Dispose();
                creatureLinesOriginPoints.Dispose();
                raycastHitPoints.Dispose();
                closestHitPoints.Dispose();
                hitPointSphereRadius.Dispose();
                scanDirections.Dispose();
                objectBitePoints.Dispose();
                objectBiteNormals.Dispose();
                isSticking.Dispose();
                pheromoneDetection.Dispose();
                builder.Dispose();
            }
        }
    }
}