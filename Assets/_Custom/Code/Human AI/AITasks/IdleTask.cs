using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class IdleTask : AITask
    {
        public IdleTask(Animator animator) : base(animator)
        {
            SetTaskConfiguration("Movement", 0f);
        }
    }
}