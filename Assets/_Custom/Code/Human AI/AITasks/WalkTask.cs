using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class WalkTask : AITask
    {
        public WalkTask(Animator animator) : base(animator)
        {
            SetTaskConfiguration("Movement", 0.5f);
        }
    }
}