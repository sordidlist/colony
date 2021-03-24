using System.Collections.Generic;
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
            
            // Define sensor raycast variables
            
            // Execute sensor raycast and store results
        }

        public void OldUpdate()
        {
            List<int> agentIndeces = new List<int>();
            float timeIncrement = Time.deltaTime;
            /*foreach (CreatureAgent.CreatureAgent creatureAgent in populationHandler.GetRandomSampleCreaturesByType(10, CreatureType.NEEDLE_ANT))
            {
                
            }*/
            for (int agentIndex = 0; agentIndex < populationHandler.GetCreatureAgentCount(); agentIndex++)
            {
                //CreatureAgent.CreatureAgent creatureAgent = populationHandler.GetCreatureAgent(agentIndex);
                /*creatureAgent.raycastTimer += timeIncrement +
                                              Random.Range(0f,
                                                  CreatureSystemConfigs.MAX_RANDOM_RAYCAST_TIMER_OFFSET);
                if (creatureAgent.raycastTimer >= CreatureSystemConfigs.MAX_RAYCAST_TIMER)
                {
                    agentIndeces.Add(agentIndex);
                    creatureAgent.raycastTimer = 0f;
                }*/
            }

            var creatureCount = agentIndeces.Count;

            if (creatureCount > 0)
                try
                {
                    var forwardRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var leftRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var rightRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var upRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var downRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var downBackRacycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var forwardDownRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var forwardDownLeftRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var forwardDownRightRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);
                    var biteObjectNormalRaycastCommands =
                        new NativeArray<RaycastCommand>(creatureCount, Allocator.Persistent);

                    var sensorOriginPoints =
                        new NativeArray<Vector3>(creatureCount, Allocator.Persistent);
                    var objectBitePoints =
                        new NativeArray<Vector3>(creatureCount, Allocator.Persistent);
                    var layerMask = new NativeArray<LayerMask>(1, Allocator.Persistent);

                    var forwardRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var leftRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var rightRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var upRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var downRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var downBackRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var forwardDownRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var forwardDownLeftRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);
                    var forwardDownRightRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);

                    var biteObjectNormalRaycastHits =
                        new NativeArray<RaycastHit>(creatureCount, Allocator.Persistent);

                    for (var i = 0; i < agentIndeces.Count; i++)
                    {
                        /*sensorOriginPoints[i] =
                            creaturePopulationController.creatureAgents[agentIndeces[i]].gameObject.transform
                                .position;
                        objectBitePoints[i] =
                            creaturePopulationController.creatureAgents[agentIndeces[i]].objectBitePoint;*/
                    }

                    layerMask[0] = RayCastLayerMask;

                    var creatureTransformsList = new List<Transform>();
                    //for (var i = 0; i < agentIndeces.Count; i++)
                    //    creatureTransformsList.Add(creaturePopulationController.creatureAgentTranforms[i]);

                    var creatureTransformsArray = creatureTransformsList.ToArray();
                    var creatureTransformAccessArray =
                        new TransformAccessArray(creatureTransformsArray);

                    /*
                     * COMPUTE DIRECTIONS IN WHICH TO FIRE THE RAYCASTS
                     */

                    /*var getRaycastDirectionsJob =
                        new CreatureSystemJobs.GetRaycastDirectionsJob
                        {
                            forwardRaycastCommands = forwardRaycastCommands,
                            downRaycastCommands = downRaycastCommands,
                            upRaycastCommands = upRaycastCommands,
                            leftRaycastCommands = leftRaycastCommands,
                            rightRaycastCommands = rightRaycastCommands,
                            forwardDownRaycastCommands = forwardDownRaycastCommands,
                            forwardDownLeftRaycastCommands = forwardDownLeftRaycastCommands,
                            forwardDownRightRaycastCommands = forwardDownRightRaycastCommands,
                            downBackRaycastCommands = downBackRacycastCommands,
                            sensorOriginPoints = sensorOriginPoints,
                            biteObjectNormalRaycastCommands = biteObjectNormalRaycastCommands,
                            objectBitePoints = objectBitePoints,
                            layerMask = layerMask
                        };*/

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

                    for (var i = 0; i < agentIndeces.Count; i++)
                    {
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
                        //Debug.Log(creatureAgent.ToString());*/
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
                }
        }

        private void CompareRaycastHitToCurrentClosest(RaycastHit raycastHit, CreatureAgent.CreatureAgent creatureAgent)
        {
            //if (raycastHit.distance != 0)
                //if (!raycastHit.collider.gameObject.Equals(gameObject))
                    //if (creatureAgent.closestRaycastHit.distance > raycastHit.distance)
                    //    creatureAgent.closestRaycastHit = raycastHit;
        }
    }
}