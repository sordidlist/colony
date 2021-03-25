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

        public void Update()
        {
            currentActiveCreatureAgents = populationHandler.GetCreatureAgentCount();
        }
    }
}