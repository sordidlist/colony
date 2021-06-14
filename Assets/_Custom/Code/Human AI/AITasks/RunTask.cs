using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class RunTask : AITask
    {
        public RunTask(Animator animator) : base(animator)
        {
            SetAnimatorParameter("Movement", 1f);
            SetAnimatorParameter("Rotate", 0f);
        }
    }
}