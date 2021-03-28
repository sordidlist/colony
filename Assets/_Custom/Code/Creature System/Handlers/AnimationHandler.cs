using System;
using System.Collections.Generic;
using GPUInstancer;
using GPUInstancer.CrowdAnimations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class AnimationHandler : Singleton<AnimationHandler>
    {
        private bool debugMode = true;

        public GPUICrowdManager crowdManager;
        public List<GPUICrowdPrototype> crowdPrototypeFilter;

        public AnimationClip antWalk;
        
        public AnimationHandler()
        {
            if (debugMode) {Debug.Log("Created Animation Handler.");}
        }
        
        public void SetInstanceMovementSpeed(CreatureAgent.CreatureAgent instance, AnimationClip animationClip, float startTime)
        {
            GPUICrowdPrefab crowdPrefab = instance.GetGPUICrowdPrefab();
            GPUICrowdAPI.StartAnimation(crowdPrefab, animationClip, startTime);
        }

        public void SetRotateSpeed(CreatureAgent.CreatureAgent instance)
        {
            throw new NotImplementedException();
        }

        public void TriggerInstanceBite(CreatureAgent.CreatureAgent instance)
        {
            throw new NotImplementedException();
        }

        public void TriggerInstanceAttack(CreatureAgent.CreatureAgent instance)
        {
            throw new NotImplementedException();
        }

        public void TriggerInstanceDeath(CreatureAgent.CreatureAgent instance)
        {
            throw new NotImplementedException();
        }
        
        private Dictionary<GPUInstancerPrototype, List<GPUInstancerPrefab>> GetRegisteredCrowdPrefabInstances()
        {
            var registeredPrefabInstances =
                crowdManager.GetRegisteredPrefabsRuntimeData();
            return registeredPrefabInstances;
        }

        private void PlayAnimationOnCreatureInstance(GPUICrowdPrefab crowdInstance, AnimationClip clipData,
            float startTime)
        {
            GPUICrowdAPI.StartAnimation(crowdInstance, clipData, startTime);
        }

        [Button("Everybody Walk")]
        public void EverybodyWalk()
        {
            if (crowdManager == null)
                crowdManager = GameObject.Find("GPUI Crowd Manager").GetComponent<GPUICrowdManager>();

            if (crowdManager != null)
            {
                var registeredPrefabInstances =
                    crowdManager.GetRegisteredPrefabsRuntimeData();

                GPUIAnimationClipData clipData;
                var startTime = 0f;
                if (registeredPrefabInstances != null)
                    foreach (GPUICrowdPrototype crowdPrototype in registeredPrefabInstances.Keys)
                    {
                        if (crowdPrototypeFilter != null && crowdPrototypeFilter.Count > 0 &&
                            !crowdPrototypeFilter.Contains(crowdPrototype))
                            continue;
                        if (crowdPrototype.animationData != null && crowdPrototype.animationData.useCrowdAnimator)
                            for (var i = 0; i < registeredPrefabInstances[crowdPrototype].Count; i++)
                                try
                                {
                                    var crowdInstance = registeredPrefabInstances[crowdPrototype][i]
                                        .gameObject.GetComponent<GPUICrowdPrefab>();
                                    clipData = crowdPrototype.animationData.clipDataList[1];

                                    GPUICrowdAPI.StartAnimation(crowdInstance, clipData.animationClip, startTime);
                                }
                                catch (NullReferenceException e)
                                {
                                    //Debug.Log(e);
                                    //Debug.Log("i: " + i);
                                }
                    }
            }
        }
    }
}