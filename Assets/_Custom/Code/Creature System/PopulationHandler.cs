using System.Collections.Generic;
using _Custom.Code.Creature_System.Utilities;

namespace _Custom.Code.Creature_System
{
    public class PopulationHandler : Singleton<PopulationHandler>
    {
        public List<CreatureAgent.CreatureAgent> registeredCreatureAgents;
        public List<CreatureAgent.CreatureAgent> batchedCreatureAgents;

        public void Start()
        {
            registeredCreatureAgents = new List<CreatureAgent.CreatureAgent>();
            batchedCreatureAgents = new List<CreatureAgent.CreatureAgent>();
        }

        public void Update()
        {
            // Draw a simple random sample of all active creature agents
            int sampleSize = GetCreatureAgentCount();
            if (sampleSize > CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE)
                sampleSize = CreatureAgentConfig.MAXIMUM_CREATURE_AGENT_BATCH_SIZE;
            
            batchedCreatureAgents.Clear();
            
            
        }

        public int GetCreatureAgentCount()
        {
            return registeredCreatureAgents.Count;
        }

        public void RegisterCreatureAgent(CreatureAgent.CreatureAgent creatureAgent)
        {
            registeredCreatureAgents.Add(creatureAgent);
        }
    }
}