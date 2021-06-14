using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class IdleTask : AITask
    {
        public IdleTask(Animator animator) : base(animator)
        {
            SetAnimatorParameter("Movement", 0f);
            SetAnimatorParameter("Rotate", 0f);
        }
    }
}