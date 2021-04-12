using _Custom.Code.Creature_System.Debug_System;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    [RequireComponent(typeof(AnimationHandler))]
    [RequireComponent(typeof(DecisionHandler))]
    [RequireComponent(typeof(InteractionHandler))]
    [RequireComponent(typeof(NavigationHandler))]
    [RequireComponent(typeof(PheromoneHandler))]
    [RequireComponent(typeof(PopulationHandler))]
    [RequireComponent(typeof(SensorRaycastHandler))]
    [RequireComponent(typeof(SpawnHandler))]
    [RequireComponent(typeof(StickyHandler))]
    [RequireComponent(typeof(StickyGlobHandler))]
    [RequireComponent(typeof(DebugHandler))]
    [RequireComponent(typeof(MetricsHandler))]
    public class CreatureSystem : Singleton<MonoBehaviour>
    {
        private AnimationHandler animationHandler;
        private DecisionHandler decisionHandler;
        private InteractionHandler interactionHandler;
        private NavigationHandler navigationHandler;
        private PheromoneHandler pheromoneHandler;
        private PopulationHandler populationHandler;
        private SensorRaycastHandler sensorRaycastHandler;
        private SpawnHandler spawnHandler;
        private StickyHandler stickyHandler;
        private StickyGlobHandler stickyGlobHandler;
        private DebugHandler debugHandler;
        private MetricsHandler metricsHandler;

        public void Awake()
        {
            GetAllHandlers();
        }

        private void GetAllHandlers()
        {
            animationHandler = GetComponent<AnimationHandler>();
            interactionHandler = GetComponent<InteractionHandler>();
            
            pheromoneHandler = GetComponent<PheromoneHandler>();
            populationHandler = GetComponent<PopulationHandler>();
            
            sensorRaycastHandler = GetComponent<SensorRaycastHandler>();
            sensorRaycastHandler.SetPopulationHandler(populationHandler);
            
            spawnHandler = GetComponent<SpawnHandler>();
            
            stickyHandler = GetComponent<StickyHandler>();
            stickyHandler.SetHandlers(populationHandler, sensorRaycastHandler);

            stickyGlobHandler = GetComponent<StickyGlobHandler>();
            stickyGlobHandler.SetHandlers(populationHandler, sensorRaycastHandler);
            
            navigationHandler = GetComponent<NavigationHandler>();
            navigationHandler.SetHandlers(populationHandler, pheromoneHandler);
            
            decisionHandler  = GetComponent<DecisionHandler>();
            decisionHandler.SetHelperHandlers(navigationHandler, pheromoneHandler, sensorRaycastHandler, stickyHandler, 
                animationHandler, spawnHandler);

            debugHandler = GetComponent<DebugHandler>();
            debugHandler.SetHandlers(sensorRaycastHandler, populationHandler);
            
            metricsHandler = GetComponent<MetricsHandler>();
            metricsHandler.SetHelperHandlers(animationHandler, decisionHandler, navigationHandler, pheromoneHandler, 
                populationHandler);
        }
    }
}