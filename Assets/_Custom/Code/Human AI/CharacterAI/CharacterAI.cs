using System;
using _Custom.Code.AITasks;
using _Custom.Code.Human_AI.AITasks;
using DataStructures.PriorityQueue;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

namespace _Custom.Code
{
    [RequireComponent(typeof(AIPath))]
    [RequireComponent(typeof(Seeker))]
    public class CharacterAI : MonoBehaviour
    {
        public CharacterProperties characterProperties;
        public PriorityQueue<AITask, AITaskPriority> aiTasks;
        public float decisionTimer;
        private Animator animator;
        public RectTransform taskIcon;
        public Transform taskIconTrackingTransform;
        private AIPath aiPath;
        private Seeker seeker;

        public void Start()
        {
            characterProperties = new CharacterProperties();
            animator = GetComponent<Animator>();
            aiPath = GetComponent<AIPath>();
            seeker = GetComponent<Seeker>();
            decisionTimer = 0.0f;
            aiPath.canMove = false;
            aiPath.enableRotation = false;
            
            AITaskPriority lowestTaskPriority = new AITaskPriority(TaskPriorityConfigs.IDLE_TASK_PRIORITY);
            aiTasks = new PriorityQueue<AITask, AITaskPriority>(lowestTaskPriority);
            taskIcon = GameObject.Find("Icons Canvas").transform.Find(gameObject.name + "TaskIcon").GetComponent<RectTransform>();
            taskIconTrackingTransform = transform.Find("Task Icon Target");

            NavigateTask navigateTask = new NavigateTask(animator, seeker, GameObject.Find("Cube").transform);
            AITaskPriority navigateTaskPriority = new AITaskPriority(TaskPriorityConfigs.NAVIGATE_TASK_PRIORITY);
            aiTasks.Insert(navigateTask, navigateTaskPriority);
        }

        public void Update()
        {
            if (IsTimeForTheNextTask())
            {
                try
                {
                    BounceTaskIconIn();
                    AITask currentTask = aiTasks.Pop();
                    currentTask.AnimateTask();
                    decisionTimer = 0f;
                    BounceTaskIconOut();
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

        private void BounceTaskIconIn()
        {
            taskIconTrackingTransform.DOScaleX(0.5f, 1f);
            taskIconTrackingTransform.DOScaleY(0.5f, 1f);
        }
        
        private void BounceTaskIconOut()
        {
            taskIconTrackingTransform.DOScaleX(0f, 1f);
            taskIconTrackingTransform.DOScaleY(0f, 1f);
        }
    }
}