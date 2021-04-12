using System.Collections.Generic;

namespace _Custom.Code.Creature_System
{
    public class NavigationHandler : Singleton<NavigationHandler>
    {
        private PopulationHandler populationHandler;
        private PheromoneHandler pheromoneHandler;

        public void SetHandlers(PopulationHandler populationHandler, PheromoneHandler pheromoneHandler)
        {
            this.populationHandler = populationHandler;
            this.pheromoneHandler = pheromoneHandler;
        }

        public void FixedUpdate()
        {
            List<CreatureAgent.CreatureAgent> batchedCreatureAgents = populationHandler.batchedCreatureAgents;
            for (int creatureAgentIndex = 0; creatureAgentIndex < batchedCreatureAgents.Count; creatureAgentIndex++)
            {
                
            }
        }
    }
}