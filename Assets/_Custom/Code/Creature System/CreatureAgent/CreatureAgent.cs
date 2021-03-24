using GPUInstancer.CrowdAnimations;
using UnityEngine;

namespace _Custom.Code.Creature_System.CreatureAgent
{
    public class CreatureAgent : MonoBehaviour
    {
        private GPUICrowdPrefab gpuiCrowdPrefab;
        private CreatureType creatureType;

        private Vector3 destination;

        public void Awake()
        {
            gpuiCrowdPrefab = GetComponent<GPUICrowdPrefab>();
            SetCreatureType();
        }

        public GPUICrowdPrefab GetGPUICrowdPrefab()
        {
            return gpuiCrowdPrefab;
        }

        public Vector3 GetDestination()
        {
            Vector3 destination = this.destination;
            return destination;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        public CreatureType GetCreatureType()
        {
            return creatureType;
        }

        public void SetCreatureType()
        {
            CreatureType creatureType = CreatureType.UNDEFINED;
            if (gameObject.name.Contains("Garden Ant"))
            {
                creatureType = CreatureType.GARDEN_ANT;
            } else if (gameObject.name.Contains("Needleant"))
            {
                creatureType = CreatureType.NEEDLE_ANT;
            }

            if (creatureType.Equals(CreatureType.UNDEFINED))
            {
                Debug.LogWarning("Undefined creature " + gameObject.name + " encountered!");
            }
            this.creatureType = creatureType;
        }
    }
}