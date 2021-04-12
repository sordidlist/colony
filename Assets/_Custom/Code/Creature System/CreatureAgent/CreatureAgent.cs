using System;
using System.Collections.Generic;
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
        
        // Sticking
        private RaycastHit closestRaycastHit;
        private bool isSticking;
        private bool isFalling;     // Creatures can be stuck together while still falling through the air
        
        // Navigation
        private Vector3 lastDetectedFoodPheromonePosition;
        private float lastDetectedFoodPheromoneTime;
        private Vector3 lastDetectedFearPheromonePosition;
        private float lastDetectedFearPheromoneTime;
        
        // Pheromones
        public float detectsFood;
        public float detectsThreat;

        public Rigidbody rigidbody;
        public PopulationHandler populationHandler;

        private Vector3 destination;
        private List<CreaturePrioritySetting> prioritySettings;

        public void Awake()
        {
            gpuiCrowdPrefab = GetComponent<GPUICrowdPrefab>();
            if (rigidbody) rigidbody.useGravity = false;
            else Debug.Log(this.name + " needs a rigidbody!");
            SetCreatureType();
            EstablishPriorities();
            CheckPopulationHandler();
            RegisterCreatureAgent();
        }

        private void EstablishPriorities()
        {
            prioritySettings = new List<CreaturePrioritySetting>();
            if (creatureType.Equals(CreatureType.NEEDLE_ANT))
            {
                prioritySettings.Add(new CreaturePrioritySetting(CreaturePriority.GATHER_FOOD, CreaturePriorityValue.AVERAGE));
                prioritySettings.Add(new CreaturePrioritySetting(CreaturePriority.EXPAND_COLONY, CreaturePriorityValue.LOW));
                prioritySettings.Add(new CreaturePrioritySetting(CreaturePriority.ESCAPE_THREAT, CreaturePriorityValue.URGENT));
                prioritySettings.Add(new CreaturePrioritySetting(CreaturePriority.PROTECT_COLONY, CreaturePriorityValue.HIGH));
            }
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

        public float GetMaximumStickyDistance()
        {
            if (creatureType.Equals(CreatureType.GARDEN_ANT)) return 1f;
            return 1f;
        }

        public bool IsSticking()
        {
            return isSticking;
        }

        public bool IsFalling()
        {
            return isFalling;
        }

        public void SetIsFalling(bool isFalling)
        {
            this.isFalling = isFalling;
        }

        public Vector3 GetLastDetectedFoodPheromonePosition()
        {
            return lastDetectedFoodPheromonePosition;
        }

        public void SetLastDetectedFoodPheromonePosition(Vector3 foodPheromonePosition)
        {
            lastDetectedFoodPheromonePosition = foodPheromonePosition;
        }

        public void SetClosestRaycastHit(RaycastHit closestRaycastHit)
        {
            this.closestRaycastHit = closestRaycastHit;
        }

        public RaycastHit GetClosestRaycastHit()
        {
            return closestRaycastHit;
        }

        public void SetIsSticking(bool isSticking)
        {
            this.isSticking = isSticking;
        }

        public void SetRigidbody(Rigidbody rigidbody)
        {
            this.rigidbody = rigidbody;
            
        }

        public Rigidbody GetRigidbody()
        {
            return this.rigidbody;
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

        public void OnTriggerStay(Collider other)
        {
            if (other.isTrigger)
            {
                if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Food Pheromone")))
                {
                    detectsFood = 1f;
                    lastDetectedFoodPheromonePosition = gameObject.transform.position;
                } 
                else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Fear Pheromone")))
                {
                    detectsThreat = 1f;
                    lastDetectedFearPheromonePosition = gameObject.transform.position;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
            {
                if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Food Pheromone")))
                {
                    detectsFood = 0f;
                } 
                else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Fear Pheromone")))
                {
                    detectsThreat = 0f;
                }
            }
        }

        public float GetDetectsFood()
        {
            return detectsFood;
        }

        public float GetDetectsThreat()
        {
            return detectsThreat;
        }
    }
}