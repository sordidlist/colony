using System.Collections.Generic;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class StickyGlobHandler : Singleton<MetricsHandler>
    {
        private PopulationHandler populationHandler;
        private SensorRaycastHandler sensorRaycastHandler;

        public void FixedUpdate()
        {
            List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;

            for (int creatureAgentIndex = 0; creatureAgentIndex < batchedCreatureAgents.Count; creatureAgentIndex++)
            {
                
            }
        }

        public void SetHandlers(PopulationHandler populationHandler, SensorRaycastHandler sensorRaycastHandler)
        {
            this.populationHandler = populationHandler;
            this.sensorRaycastHandler = sensorRaycastHandler;
        }

        public void OnCollisionEnter(Collision other)
        {
            DetermineIfFalling(other);
        }

        private void DetermineIfFalling(Collision collision)
        {
            
        }
    }

    class StickyGlob
    {
        public List<int> creatureAgentIndexList;

        public bool GlobContainsCreatureAgentIndex(int creatureAgentIndex)
        {
            return creatureAgentIndexList.Contains(creatureAgentIndex);
        }
    }
}