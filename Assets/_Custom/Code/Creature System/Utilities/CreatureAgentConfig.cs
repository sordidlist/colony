using UnityEngine;

namespace _Custom.Code.Creature_System.Utilities
{
    public static class CreatureAgentConfig
    {
        public static bool USE_MAXIMUM_CREATURE_AGENT_BATCH_SIZE = true;
        public static int MAXIMUM_CREATURE_AGENT_BATCH_SIZE = 1000;

        public static Vector3[] DIRECTONS_TO_CAST_SENSOR_RAYCASTS_FROM_CREATURE_AGENTS =
        {
            Vector3.forward,
            Vector3.left,
            Vector3.right, 
            Vector3.up,
            Vector3.down,
            Vector3.down + Vector3.back,
            Vector3.forward + Vector3.down,
            Vector3.forward + Vector3.down + Vector3.left,
            Vector3.forward + Vector3.down + Vector3.right,
        };

        public static float MAX_CREATURE_AGENT_SCAN_DISTANCE = 10f;
        public static int SENSOR_RAYCAST_PARALLEL_JOBS_COUNT = 32;
    }
}