using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace _Custom.Code.Human_AI.AITasks
{
    public class NavigateTask : AITask
    {
        public List<Vector3> currentPath;
        
        public NavigateTask(Animator animator, Seeker seeker, Transform destination) : base(animator, destination)
        {
            Transform characterTransform = animator.gameObject.transform;
            QueueNewPathRequest(seeker, characterTransform, destination.position);
            float destinationAngle =
                Vector3.SignedAngle(characterTransform.forward, aiTaskTransform.position, Vector3.up);
            SetAnimatorParameter("Rotate", destinationAngle);
        }

        public void QueueNewPathRequest(Seeker seeker, Transform characterTransform, Vector3 destination)
        {
            seeker.StartPath(characterTransform.position, destination, OnPathComplete);
        }

        public void OnPathComplete(Path p)
        {
            if (p.error)
                Debug.Log(" Pathfinding Error: " + p.error);
            else
                currentPath = p.vectorPath;
        }
    }
}