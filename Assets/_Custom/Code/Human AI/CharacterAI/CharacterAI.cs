using System;
using _Custom.Code.AITasks;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace _Custom.Code
{
    public class CharacterAI : MonoBehaviour
    {
        public CharacterProperties characterProperties;
        public PriorityQueue<AITask, AITaskPriority> aiTasks;
        public float decisionTimer;
        private Animator animator;

        public void Start()
        {
            characterProperties = new CharacterProperties();
            animator = GetComponent<Animator>();
            decisionTimer = 0.0f;
            
            AITaskPriority lowestTaskPriority = new AITaskPriority(TaskPriorityConfigs.IDLE_TASK_PRIORITY);
            aiTasks = new PriorityQueue<AITask, AITaskPriority>(lowestTaskPriority);
        }

        public void Update()
        {
            if (IsTimeForTheNextTask())
            {
                try
                {
                    aiTasks.Pop().AnimateTask();
                    decisionTimer = 0f;
                }
                catch (NullReferenceException e)
                {
                    AITask idleTask = new IdleTask(animator);
                    AITaskPriority idleTaskPriority = new AITaskPriority(TaskPriorityConfigs.IDLE_TASK_PRIORITY);
                    aiTasks.Insert(idleTask, idleTaskPriority);
                    EvaluateWhatTheNextTaskShouldBe();
                }
            }

            decisionTimer += Time.deltaTime;
        }

        private bool IsTimeForTheNextTask()
        {
            return decisionTimer >= HumanAIConfigs.MAX_DURATION_BETWEEN_TASK_CHANGES;
        }

        private void EvaluateWhatTheNextTaskShouldBe()
        {
            
        }
    }
}