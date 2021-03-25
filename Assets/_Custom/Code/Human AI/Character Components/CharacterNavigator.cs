using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace _Custom.Code
{
    public class CharacterNavigator : MonoBehaviour
    {
        private Seeker seeker;
        private AIPath aiPath;

        public Vector3 start;
        public Vector3 end;

        public GameObject seekTargetObject;

        public List<Vector3> currentPath;

        private float repathTimer;
        private float repathRate = 1f;

        public float currentDesiredTurnAngle;
        private float turnAroundAngleThreshold = 145f;
        private float turnHalfAngleThreshold = 55f;
        private float turnSmallAngleThreshold = 55f;

        public float distanceToEnd;

        public void Start()
        {
            ValidateNecessaryComponents();
        }

        private void ValidateNecessaryComponents()
        {
            aiPath = GetComponent<AIPath>();
            seeker = GetComponent<Seeker>();
            if (currentPath == null)
                currentPath = new List<Vector3>();
        }

        public bool HasActivePath()
        {
            return currentPath.Count > 0;
        }

        public void Update()
        {
            repathTimer += Time.deltaTime;

            if (seekTargetObject)
            {
                end = seekTargetObject.transform.position;
            }

            if (end != Vector3.zero)
            {
                EvaluateCurrentDesiredTurnAngle();
                AdjustAnimatorControllerTurnValuesForTurns();
                UpdatePathData();

                distanceToEnd = Vector3.Distance(end, transform.position);
            }

            if (repathTimer > repathRate)
                repathTimer = 0f;
        }

        private void UpdatePathData()
        {
            if (repathTimer > repathRate)
            {
                repathTimer = 0f;
                QueueNewPathRequest(end);
            }
        }

        private void EvaluateCurrentDesiredTurnAngle()
        {
            Vector3 targetDirection;
            if (currentPath.Count > 3)
                targetDirection = currentPath[3] - transform.position;
            else
                targetDirection = end - transform.position;

            currentDesiredTurnAngle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
        }

        private void AdjustAnimatorControllerTurnValuesForTurns()
        {
            // if negative, turn right
            // if positive, turn left
            
            float rotationSpeed = Mathf.Abs(currentDesiredTurnAngle / 180);
            transform.Rotate(0f, -currentDesiredTurnAngle * Time.deltaTime * rotationSpeed, 0f);
        }

        public void QueueNewPathRequest(Vector3 destination)
        {
            start = transform.position;
            end = destination;
            seeker.StartPath(start, end, OnPathComplete);
        }

        public void OnPathComplete(Path p)
        {
            if (p.error)
            {
                Debug.Log(gameObject.name + " Pathfinding Error: " + p.error);
            }
            else
            {
                currentPath = p.vectorPath;
            }
        }
    }
}