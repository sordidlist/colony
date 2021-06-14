using UnityEngine;

namespace _Custom.Code.Human_AI.AITasks
{
    public class NavigateTask : AITask
    {
        public NavigateTask(Animator animator, Vector3 destination) : base(animator, destination)
        {
            SetAnimatorParameter("Movement", 0.5f);
            GameObject character = animator.gameObject;
            float destinationAngle =
                Vector3.SignedAngle(character.transform.position, aiTaskPosition, Vector3.up);
            Debug.Log(destinationAngle);
            SetAnimatorParameter("Rotate", destinationAngle);
        }
    }
}