using System;
using UnityEngine;

namespace _Custom.Code
{
    public abstract class AITask
    {
        public AITask(Animator animator)
        {
            this.animator = animator;
        }
        
        public AITask(Animator animator, Transform aiTaskTransform)
        {
            this.animator = animator;
            this.aiTaskTransform = aiTaskTransform;
        }

        public Animator animator;
        public Transform aiTaskTransform;
        private AITaskAnimatorConfiguration aiTaskAnimatorConfiguration;
        private ParameterType parameterType;

        class AITaskAnimatorConfiguration
        {
            public AITaskAnimatorConfiguration(string parameterName, float parameterValue)
            {
                this.parameterName = parameterName;
                parameterValueFloat = parameterValue;
            }

            public AITaskAnimatorConfiguration(string parameterName, bool parameterValue)
            {
                this.parameterName = parameterName;
                parameterValueBool = parameterValue;
            }

            public string parameterName;
            public float parameterValueFloat;
            public bool parameterValueBool;
        }

        enum ParameterType
        {
            FLOAT,
            BOOL,
            TRIGGER,
        }

        public void AnimateTask()
        {
            if (parameterType.Equals(ParameterType.FLOAT))
            {
                animator.SetFloat(aiTaskAnimatorConfiguration.parameterName,
                    aiTaskAnimatorConfiguration.parameterValueFloat);
            }
            else if (parameterType.Equals(ParameterType.BOOL))
            {
                animator.SetBool(aiTaskAnimatorConfiguration.parameterName,
                    aiTaskAnimatorConfiguration.parameterValueBool);
            }
        }

        public void SetAnimatorParameter(string parameterName, float parameterValue)
        {
            aiTaskAnimatorConfiguration = new AITaskAnimatorConfiguration(parameterName, parameterValue);
            parameterType = ParameterType.FLOAT;
        }

        public void SetAnimatorParameter(string parameterName, bool parameterValue)
        {
            aiTaskAnimatorConfiguration = new AITaskAnimatorConfiguration(parameterName, parameterValue);
            parameterType = ParameterType.BOOL;
        }
    }


    public class AITaskPriority : IComparable<AITaskPriority>
    {
        public AITaskPriority(int priorityValue)
        {
            this.priorityValue = priorityValue;
        }

        public int priorityValue;

        public int CompareTo(AITaskPriority other)
        {
            if (other.priorityValue > priorityValue)
            {
                return 1;
            }

            if (other.priorityValue < priorityValue)
            {
                return -1;
            }

            return 0;
        }
    }
}