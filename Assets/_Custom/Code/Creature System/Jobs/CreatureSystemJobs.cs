using _Custom.Code.Creature_System.Utilities;
using Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _Custom.Code.Creature_System
{
    public class CreatureSystemJobs
    {
        [BurstCompile]
        public struct GetRaycastDirectionsRelativeToCreatureAgentsTransformsJob : IJobParallelForTransform
        {
            [NativeDisableParallelForRestriction] public NativeArray<RaycastCommand> sensorRaycastCommands;
            public NativeArray<Vector3> objectBitePoints;
            public NativeArray<Vector3> sensorOriginPoints;
            
            [ReadOnly] public NativeArray<LayerMask> layerMask;
            [ReadOnly] public NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents;
            [ReadOnly] public NativeArray<float> maxCreatureAgentScanDistance;
            
            public void Execute(int index, TransformAccess transform)
            {
                for (int scanDirection = 0;
                    scanDirection < directionsToCastSensorRaycastsFromCreatureAgents.Length;
                    scanDirection++)
                {
                    sensorOriginPoints[index] = transform.position;
                    Vector3 directionRelativeToCreatureAgent = transform.rotation * 
                                                               directionsToCastSensorRaycastsFromCreatureAgents[scanDirection];
                    int sensorRaycastCommandIndex = (index * directionsToCastSensorRaycastsFromCreatureAgents.Length) +
                        scanDirection;
                    RaycastCommand directionRaycastCommand
                        = new RaycastCommand(sensorOriginPoints[index],
                        directionRelativeToCreatureAgent,
                        maxCreatureAgentScanDistance[0], layerMask[0]);
                    sensorRaycastCommands[sensorRaycastCommandIndex] = directionRaycastCommand;
                }
            }
        }
        
        [BurstCompile]
        public struct GetNearestRaycastHitsJob : IJobParallelForTransform
        {
            public NativeArray<RaycastHit> closestRaycastHits;
            
            [ReadOnly] public NativeArray<RaycastHit> sensorRaycastHits;
            [ReadOnly] public NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents;
            
            public void Execute(int index, TransformAccess transform)
            {
                RaycastHit closestRaycastHit = new RaycastHit();
                float closestRaycastHitDistance = 99999;
                for (int creatureAgentRaycastHitsIndex = 0;
                    creatureAgentRaycastHitsIndex < directionsToCastSensorRaycastsFromCreatureAgents.Length;
                    creatureAgentRaycastHitsIndex++)
                {
                    if ((!sensorRaycastHits[(index * directionsToCastSensorRaycastsFromCreatureAgents.Length) + creatureAgentRaycastHitsIndex].point.Equals(Vector3.zero)) && 
                        (sensorRaycastHits[(index * directionsToCastSensorRaycastsFromCreatureAgents.Length) + creatureAgentRaycastHitsIndex].distance < closestRaycastHitDistance))
                    {
                        closestRaycastHitDistance =
                            sensorRaycastHits[
                                (index * directionsToCastSensorRaycastsFromCreatureAgents.Length) +
                                creatureAgentRaycastHitsIndex].distance;
                        closestRaycastHit =
                            sensorRaycastHits[
                                (index * directionsToCastSensorRaycastsFromCreatureAgents.Length) +
                                creatureAgentRaycastHitsIndex];
                    }
                    closestRaycastHits[index] = closestRaycastHit;
                }
            }
        }
        
        [BurstCompile]
        public struct CheckCreatureAgentStickinessJob : IJobParallelFor
        {
            public NativeArray<RaycastHit> closestHitPoints;
            public NativeArray<float> maxStickyDistance;
            public NativeArray<bool> isSticking;
            
            public void Execute(int index)
            {
                if (closestHitPoints[index].point.Equals(Vector3.zero)) isSticking[index] = false;
                else isSticking[index] = closestHitPoints[index].distance <= maxStickyDistance[index];
            }
        }
        
        [BurstCompile]
        public struct DrawingJob : IJob
        {
            public CommandBuilder builder;
            [ReadOnly] public NativeArray<Vector3> origin;
            [ReadOnly] public NativeArray<float> sphereRadius;
            [ReadOnly] public NativeArray<Vector3> directionsToScan;
            [ReadOnly] public NativeArray<Vector3> objectBitePoints;
            [ReadOnly] public NativeArray<Vector3> objectBiteNormals;
            [ReadOnly] public NativeArray<Color> lineColors;
            [ReadOnly] public NativeArray<bool> isSticking;

            public NativeArray<RaycastHit> raycastHitPoints;
            public NativeArray<RaycastHit> closestHitPoints;

            public void Execute()
            {
                for (var originIndex = 0; originIndex < origin.Length; originIndex++)
                {
                    var color = 0;
                    for (var hitPointIndex = originIndex * directionsToScan.Length;
                        hitPointIndex < originIndex * directionsToScan.Length + directionsToScan.Length;
                        hitPointIndex++)
                    {
                        if (raycastHitPoints.Length > 0 && raycastHitPoints[hitPointIndex].point != Vector3.zero)
                        {
                            builder.Line(origin[originIndex], raycastHitPoints[hitPointIndex].point, lineColors[color]);
                            builder.WireSphere(raycastHitPoints[hitPointIndex].point, sphereRadius[0],
                                lineColors[color]);
                        }
                        /*else
                        {
                            builder.Line(origin[originIndex],
                                origin[originIndex] + rotation[originIndex] * directionsToScan[color],
                                lineColors[color]);
                                
                        }*/

                        color++;
                    }

                    if (objectBitePoints[originIndex] != Vector3.zero)
                    {
                        builder.Line(origin[originIndex], objectBitePoints[originIndex], Color.black);
                        builder.WireBox(objectBitePoints[originIndex], Quaternion.identity, sphereRadius[0]);

                        builder.Line(objectBitePoints[originIndex],
                            objectBitePoints[originIndex] + objectBiteNormals[originIndex], Color.red + Color.yellow);
                    }

                    builder.WireSphere(closestHitPoints[originIndex].point, sphereRadius[0] * 0.5f,
                        DebugSystemConfig.CLOSEST_HIT_POINT_LINE_COLOR);
                    
                    if (isSticking[originIndex])
                        builder.WireSphere(origin[originIndex], sphereRadius[0],
                            DebugSystemConfig.IS_STICKING_COLOR);
                    else
                        builder.WireSphere(origin[originIndex], sphereRadius[0],
                            DebugSystemConfig.IS_NOT_STICKING_COLOR);
                }
            }
        }
    }
}