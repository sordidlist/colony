using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class MetricsHandler : Singleton<MetricsHandler>
    {
        private AnimationHandler animationHandler;
        private DecisionHandler decisionHandler;
        private NavigationHandler navigationHandler;
        private PheromoneHandler pheromoneHandler;
        private PopulationHandler populationHandler;

        public int currentActiveCreatureAgents;
        public int inactiveCreatureAgents;

        private int batchSizePointsCount = 10000;
        private int sumOfLast10kBatchSizes;
        public float averageCreaturesPerLast10kBatches;
        private Queue<int> batchSizeHistogram10kPoints;

        public void Awake()
        {
            batchSizeHistogram10kPoints = new Queue<int>();
        }

        public void SetHelperHandlers(AnimationHandler animationHandler, DecisionHandler decisionHandler,
            NavigationHandler navigationHandler,
            PheromoneHandler pheromoneHandler, PopulationHandler populationHandler)
        {
            this.animationHandler = animationHandler;
            this.decisionHandler = decisionHandler;
            this.navigationHandler = navigationHandler;
            this.pheromoneHandler = pheromoneHandler;
            this.populationHandler = populationHandler;
        }

        public void FixedUpdate()
        {
            currentActiveCreatureAgents = populationHandler.GetCreatureAgentCount();
            UpdateBatchSizeHistogramData();
        }

        private void UpdateBatchSizeHistogramData()
        {
            if (batchSizeHistogram10kPoints.Count >= batchSizePointsCount)
            {
                sumOfLast10kBatchSizes -= batchSizeHistogram10kPoints.Dequeue();
            }
            batchSizeHistogram10kPoints.Enqueue(populationHandler.GetCurrentCreatureAgentBatchSize());
            sumOfLast10kBatchSizes += populationHandler.GetCurrentCreatureAgentBatchSize();
            averageCreaturesPerLast10kBatches = sumOfLast10kBatchSizes / (float) batchSizeHistogram10kPoints.Count;
        }
    }
}