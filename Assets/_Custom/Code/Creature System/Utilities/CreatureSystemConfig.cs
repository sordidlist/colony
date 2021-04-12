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
        public static float DEFAULT_MAGNETIC_ROTATION_SPEED_GROUNDED = 20f;
        public static float DEFAULT_GRAVITY_SPEED = 500f;
        public static float STICKING_GRAVITY_SPEED = 25000f;

        public static float MAX_DURATION_UNTIL_FORGET_FOOD_PHEROMONE = 10f;
    }
    
    public static class DebugSystemConfig 
    {
        public static int LINE_DRAWER_COMMAND_BUILDER_ALLOCATE_SIZE = 100;
        public static bool START_DEBUG_HANDLER = true;
        public readonly static Color CLOSEST_HIT_POINT_LINE_COLOR = Color.green;
        public static float DEBUG_RAYCASTHIT_SPHERE_COLLIDER_RADIUS = 1.0f;
        public readonly static Color IS_STICKING_COLOR = Color.blue;
        public readonly static Color IS_NOT_STICKING_COLOR = Color.magenta;
        public readonly static Color IS_STICKING_AND_FALLING_COLOR = Color.blue + Color.white;

        public readonly static Color DETECTS_FOOD = Color.yellow + Color.red;
        public readonly static Color DETECTS_THREAT = Color.red + Color.magenta + Color.blue;
        public readonly static Color DETECTS_NOTHING = Color.gray;
        public readonly static Color DETECTS_MULTIPLE = Color.cyan + Color.yellow;

        public readonly static float PHEROMONE_INDICATOR_OFFSET = 0.5f;
    }
}