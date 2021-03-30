using GPUInstancer.CrowdAnimations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Custom.Code.Creature_System.CreatureAgent
{
    public class CreatureAgent : MonoBehaviour
    {
        private GPUICrowdPrefab gpuiCrowdPrefab;
        private CreatureType creatureType;
        private bool registered;

        public PopulationHandler populationHandler;

        private Vector3 destination;

        public void Awake()
        {
            gpuiCrowdPrefab = GetComponent<GPUICrowdPrefab>();
            SetCreatureType();
            CheckPopulationHandler();
            RegisterCreatureAgent();
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

        public bool IsRegistered()
        {
            return registered;
        }

        public void SetRegistered()
        {
            registered = true;
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

        private void CheckPopulationHandler()
        {
            if (populationHandler.Equals(null))
                Debug.LogWarning("The PopulationHandler on " + gameObject.name + "'s CreatureAgent component needs " +
                                 "to be set. Please drag & drop this scene's Creature System object onto the CreatureAgent component.");
        }

        [Button("Register Creature Agent")]
        private void RegisterCreatureAgent()
        {
            if (!populationHandler.Equals(null))
            {
                if (!populationHandler.setUpComplete)
                {
                    populationHandler.SetUpPopulationHandler();                    
                }
                populationHandler.RegisterCreatureAgent(this);
            }
        }
    }
}