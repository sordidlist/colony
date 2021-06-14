using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class WalkTask : AITask
    {
        public WalkTask(Animator animator) : base(animator)
        {
            SetAnimatorParameter("Movement", 0.5f);
            SetAnimatorParameter("Rotate", 0);
        }
    }
}