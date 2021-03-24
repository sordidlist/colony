using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class DecisionHandler : Singleton<DecisionHandler>
    {
        private bool debugMode = true;

        private NavigationHandler navigationHandler;
        private PheromoneHandler pheromoneHandler;
        private SensorRaycastHandler sensorRaycastHandler;
        private StickyHandler stickyHandler;
        
        private AnimationHandler animationHandler;
        private SpawnHandler spawnHandler;

        public DecisionHandler(NavigationHandler navigationHandler, PheromoneHandler pheromoneHandler, 
            SensorRaycastHandler sensorRaycastHandler, StickyHandler stickyHandler, AnimationHandler animationHandler, 
            SpawnHandler spawnHandler)
        {
            this.navigationHandler = navigationHandler;
            this.pheromoneHandler = pheromoneHandler;
            this.sensorRaycastHandler = sensorRaycastHandler;
            this.stickyHandler = stickyHandler;
            this.animationHandler = animationHandler;
            this.spawnHandler = spawnHandler;
            
            if (debugMode) {Debug.Log("Created Decision Handler.");}
        }
        
        public void SetHelperHandlers(NavigationHandler navigationHandler, PheromoneHandler pheromoneHandler, 
            SensorRaycastHandler sensorRaycastHandler, StickyHandler stickyHandler, AnimationHandler animationHandler, 
            SpawnHandler spawnHandler)
        {
            this.navigationHandler = navigationHandler;
            this.pheromoneHandler = pheromoneHandler;
            this.sensorRaycastHandler = sensorRaycastHandler;
            this.stickyHandler = stickyHandler;
            this.animationHandler = animationHandler;
            this.spawnHandler = spawnHandler;
            
            if (debugMode) {Debug.Log("Created Decision Handler.");}
        }

        public void Update()
        {
            
        }
        
    }
}