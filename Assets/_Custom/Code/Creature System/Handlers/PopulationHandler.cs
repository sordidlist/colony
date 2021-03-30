using System.Collections.Generic;
using _Custom.Code.Creature_System.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class PopulationHandler : Singleton<PopulationHandler>
    {
        public List<CreatureAgent.CreatureAgent> registeredCreatureAgents;
        public List<CreatureAgent.CreatureAgent> batchedCreatureAgents;
        public bool setUpComplete;

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

        public void Start()
        {
            if (!setUpComplete)
                SetUpPopulationHandler();
        }

        public void SetUpPopulationHandler()
        {
            setUpComplete = true;
            if (debugMode) Debug.Log("Defining creature agent lists...");
            registeredCreatureAgents = new List<CreatureAgent.CreatureAgent>();
            batchedCreatureAgents = new List<CreatureAgent.CreatureAgent>();
            if (debugMode) Debug.Log("Creature agent lists defined.");
        }
        public void Update()
        {
            // Draw a simple random sample of all active creature agents
            int sampleSize = GetCreatureAgentCount();
            if (sampleSize > CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE && 
                CreatureAgentConfig.USE_MAXIMUM_CREATURE_AGENT_BATCH_SIZE)
                sampleSize = CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE;
            if (debugMode) Debug.Log("Current sample size: " + sampleSize);

            List<int> selectedCreatureAgentIndeces =
                GetRandomSetOfIntegersFromRange(GetCreatureAgentCount(), sampleSize);
            
            if (debugMode) Debug.Log("Calculated selected creature agent indeces for batching: " + selectedCreatureAgentIndeces);
            
            batchedCreatureAgents.Clear();
            
            PopulateBatchedCreatureAgents(selectedCreatureAgentIndeces);
        }

        public int GetCreatureAgentCount()
        {
            return registeredCreatureAgents.Count;
        }

        public void RegisterCreatureAgent(CreatureAgent.CreatureAgent creatureAgent)
        {
            if (!creatureAgent.IsRegistered())
            {
                if (debugMode) Debug.Log("Registering creature agent: " + creatureAgent.name);
                creatureAgent.SetRegistered();
                registeredCreatureAgents.Add(creatureAgent);
            }
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
                if (debugMode) Debug.Log("Populating batch with creature agent: " + i + "...");
                batchedCreatureAgents.Add(registeredCreatureAgents[selectedCreatureAgentsList[i]]);
            }
        }

        public int GetCurrentCreatureAgentBatchSize()
        {
            return batchedCreatureAgents.Count;
        }
    }
}