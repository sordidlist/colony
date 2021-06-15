using UnityEngine;

namespace _Custom.Code.Human_AI.AITasks
{
    public class NavigateTask : AITask
    {
        public NavigateTask(Animator animator, Transform destination) : base(animator, destination)
        {
            Transform characterTransform = animator.gameObject.transform;
            float destinationAngle =
                Vector3.SignedAngle(characterTransform.forward, aiTaskTransform.position, Vector3.up);
            SetAnimatorParameter("Rotate", destinationAngle);
        }
    }
}