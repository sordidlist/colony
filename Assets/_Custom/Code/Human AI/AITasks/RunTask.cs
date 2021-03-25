using UnityEngine;

namespace _Custom.Code.AITasks
{
    public class RunTask : AITask
    {
        public RunTask(Animator animator) : base(animator)
        {
            SetTaskConfiguration("Movement", 1f);
        }
    }
}