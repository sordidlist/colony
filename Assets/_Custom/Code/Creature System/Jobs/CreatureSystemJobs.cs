using _Custom.Code.Creature_System.Utilities;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace _Custom.Code.Creature_System
{
    public class CreatureSystemJobs
    {
        [BurstCompile]
        public struct GetRaycastDirsJob : IJobParallelForTransform
        {
            [ReadOnly] public NativeArray<LayerMask> layerMask;
            [ReadOnly] public NativeArray<Vector3> sensorOriginPoints;
            [ReadOnly] public NativeArray<Vector3> objectBitePoints;

            public NativeArray<RaycastCommand> forwardRaycastCommands;
            public NativeArray<RaycastCommand> upRaycastCommands;
            public NativeArray<RaycastCommand> downRaycastCommands;
            public NativeArray<RaycastCommand> downBackRaycastCommands;
            public NativeArray<RaycastCommand> leftRaycastCommands;
            public NativeArray<RaycastCommand> rightRaycastCommands;
            public NativeArray<RaycastCommand> forwardDownRaycastCommands;
            public NativeArray<RaycastCommand> forwardDownRightRaycastCommands;
            public NativeArray<RaycastCommand> forwardDownLeftRaycastCommands;
            public NativeArray<RaycastCommand> biteObjectNormalRaycastCommands;

            public NativeArray<NativeArray<RaycastCommand>> raycastCommands;

            public void Execute(int index, TransformAccess transform)
            {
                /*var directionsMap =
                    new CreatureSystemConfigs.DirectionsMap(transform.localRotation);
                forwardRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.forward, 10f, layerMask[0]);
                leftRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.left, 10f, layerMask[0]);
                rightRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.right, 10f, layerMask[0]);
                upRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.up, 10f, layerMask[0]);
                downRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.down, 10f, layerMask[0]);
                downBackRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], directionsMap.downBack, 10f, layerMask[0]);
                forwardDownRaycastCommands[index] = new RaycastCommand(sensorOriginPoints[index],
                    directionsMap.forwardDown, 10f, layerMask[0]);
                forwardDownLeftRaycastCommands[index] = new RaycastCommand(sensorOriginPoints[index],
                    directionsMap.forwardDownLeft, 10f, layerMask[0]);
                forwardDownRightRaycastCommands[index] = new RaycastCommand(sensorOriginPoints[index],
                    directionsMap.forwardDownRight, 10f, layerMask[0]);
                biteObjectNormalRaycastCommands[index] =
                    new RaycastCommand(sensorOriginPoints[index], objectBitePoints[index] - sensorOriginPoints[index],
                        10f, layerMask[0]);
                        */
            }
        }
        
        [BurstCompile]
        public struct GetRaycastDirectionsRelativeToCreatureAgentsTransformsJob : IJobParallelForTransform
        {
            [NativeDisableParallelForRestriction] public NativeArray<RaycastCommand> sensorRaycastCommands;
            public NativeArray<Vector3> objectBitePoints;
            
            [ReadOnly] public NativeArray<Vector3> sensorOriginPoints;
            [ReadOnly] public NativeArray<LayerMask> layerMask;
            [ReadOnly] public NativeArray<Vector3> directionsToCastSensorRaycastsFromCreatureAgents;
            [ReadOnly] public NativeArray<float> maxCreatureAgentScanDistance;
            
            public void Execute(int index, TransformAccess transform)
            {
                for (int scanDirection = 0;
                    scanDirection < directionsToCastSensorRaycastsFromCreatureAgents.Length;
                    scanDirection++)
                {
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
        public struct FireSensorRaycastsForAllCreatureAgentsJob : IJobParallelForTransform
        {
            public void Execute(int index, TransformAccess transform)
            {
                
            }
        }
    }
}