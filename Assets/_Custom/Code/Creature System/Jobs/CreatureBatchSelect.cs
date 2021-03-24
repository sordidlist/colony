using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace _Custom.Code.Creature_System.Jobs
{
    public class CreatureBatchSelect
    {
        [BurstCompile]
        public struct CreatureBatchSelectJob : IJobParallelForTransform
        {
            [ReadOnly] public NativeHashMap<int, float> creatureBatchTimes;
            [ReadOnly] public NativeArray<float> minimumTimeBetweenBatches;
            [ReadOnly] public NativeArray<float> timeToAddToBatchTimer;
            [ReadOnly] public NativeArray<float> randomTimerAdjustmentFraction;

            public NativeList<int> batchedCreatureAgentIndeces;

            public void Execute(int index, TransformAccess transform)
            {
                float timeSinceLastBatch = creatureBatchTimes[index];
                if (timeSinceLastBatch > minimumTimeBetweenBatches[0])
                    BatchCreatureAgent(index);
                else
                    IncrementCreatureAgentBatchTimer(index);
            }

            private void BatchCreatureAgent(int index)
            {
                creatureBatchTimes[index] = 0f;
                batchedCreatureAgentIndeces.Add(index);
            }

            private void IncrementCreatureAgentBatchTimer(int index)
            {
                float random_adjustment_boundary = timeToAddToBatchTimer[0] * randomTimerAdjustmentFraction[0];
                float random_adjustment = Random.Range(-random_adjustment_boundary, random_adjustment_boundary);
                creatureBatchTimes[index] += timeToAddToBatchTimer[0] + random_adjustment;
            }
        }
    }
}