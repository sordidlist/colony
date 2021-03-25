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
        private Animator animator;
        public float decisionTimer;

        public void Start()
        {
            characterProperties = new CharacterProperties();
            animator = GetComponent<Animator>();

            decisionTimer = 0.0f;

            AITask idleTask = new IdleTask(animator);
            AITaskPriority idleTaskPriority = new AITaskPriority(TaskPriorityConfigs.IDLE_TASK_PRIORITY);
            
            AITask walkTask = new WalkTask(animator);
            AITaskPriority walkTaskPriority = new AITaskPriority(TaskPriorityConfigs.WALK_TASK_PRIORITY);
            
            AITask runTask = new RunTask(animator);
            AITaskPriority runTaskPriority = new AITaskPriority(TaskPriorityConfigs.RUN_TASK_PRIORITY);
            aiTasks = new PriorityQueue<AITask, AITaskPriority>(idleTaskPriority);

            aiTasks.Insert(idleTask, idleTaskPriority);
            aiTasks.Insert(walkTask, walkTaskPriority);
            aiTasks.Insert(runTask, runTaskPriority);
        }

        public void Update()
        {
            if (decisionTimer >= HumanAIConfigs.MAX_DURATION_BETWEEN_TASK_CHANGES)
            {
                try
                {
                    aiTasks.Pop().AnimateTask();
                }
                catch (NullReferenceException e)
                {
                    
                }

                decisionTimer = 0f;
            }

            decisionTimer += Time.deltaTime;
        }
    }
}