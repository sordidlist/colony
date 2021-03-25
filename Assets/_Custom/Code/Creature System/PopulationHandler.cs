using System.Collections.Generic;
using _Custom.Code.Creature_System.Debug_System;
using _Custom.Code.Creature_System.Utilities;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class PopulationHandler : Singleton<PopulationHandler>
    {
        public List<CreatureAgent.CreatureAgent> registeredCreatureAgents;
        public List<CreatureAgent.CreatureAgent> batchedCreatureAgents;

        public void Awake()
        {
            registeredCreatureAgents = new List<CreatureAgent.CreatureAgent>();
            batchedCreatureAgents = new List<CreatureAgent.CreatureAgent>();
        }

        public void Update()
        {
            // Draw a simple random sample of all active creature agents
            int sampleSize = GetCreatureAgentCount();
            if (sampleSize > CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE && 
                CreatureAgentConfig.USE_MAXIMUM_CREATURE_AGENT_BATCH_SIZE)
                sampleSize = CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE;
            
            batchedCreatureAgents.Clear();

            List<int> selectedCreatureAgentIndeces =
                GetRandomSetOfIntegersFromRange(GetCreatureAgentCount(), sampleSize);
            
            PopulateBatchedCreatureAgents(selectedCreatureAgentIndeces);
        }

        public int GetCreatureAgentCount()
        {
            return registeredCreatureAgents.Count;
        }

        public void RegisterCreatureAgent(CreatureAgent.CreatureAgent creatureAgent)
        {
            registeredCreatureAgents.Add(creatureAgent);
        }

        private List<int> GetRandomSetOfIntegersFromRange(int maxValue, int sampleSize)
        {
            List<int> selectedCreatureAgentsList = new List<int>();
            
            for (int i = 0; i < maxValue; i++)
            {
                float randomSelectionChance = Random.Range(0f, 1f);

                if (selectedCreatureAgentsList.Count >= sampleSize)
                    break;
                
                float populationSampleFraction = sampleSize / (float) maxValue;

                if (randomSelectionChance < populationSampleFraction)
                    selectedCreatureAgentsList.Add(i);
            }

            return selectedCreatureAgentsList;
        }

        private void PopulateBatchedCreatureAgents(List<int> selectedCreatureAgentsList)
        {
            for (int i = 0; i < selectedCreatureAgentsList.Count; i++)
            {
                batchedCreatureAgents.Add(registeredCreatureAgents[selectedCreatureAgentsList[i]]);
            }
        }

        public int GetCurrentCreatureAgentBatchSize()
        {
            return batchedCreatureAgents.Count;
        }
    }
}