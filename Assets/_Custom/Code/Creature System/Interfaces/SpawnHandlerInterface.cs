using GPUInstancer.CrowdAnimations;
using UnityEngine;

namespace _Custom.Code.Creature_System.Interfaces
{
    public interface SpawnHandlerInterface
    {
        public void SpawnInstance(Vector3 spawnPoint);
        public void KillInstance(CreatureAgent.CreatureAgent instance);
    }
}