using _Custom.Code.Creature_System.Interfaces;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class SpawnHandlerInterfaces : ISpawnHandlerInterface
    {
        public void SpawnInstance(Vector3 spawnPoint)
        {
            throw new System.NotImplementedException();
        }

        public void KillInstance(CreatureAgent.CreatureAgent instance)
        {
            throw new System.NotImplementedException();
        }
    }
}