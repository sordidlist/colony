using UnityEngine;

namespace _Custom.Code.Creature_System
{
    public class NavigationHandler : Singleton<NavigationHandler>
    {
        public bool HasDestination(CreatureAgent.CreatureAgent instance)
        {
            Vector3 destination = instance.GetDestination();
            if (destination == Vector3.zero) return true;
            return false;
        }

        public void SetDestination(CreatureAgent.CreatureAgent instance, Vector3 destination)
        {
            instance.SetDestination(destination);
        }

        public float GetDistanceEuclidian(Vector3 start, Vector3 destination)
        {
            return Vector3.Distance(start, destination);
        }

        public float GetDestinationAngle(Vector3 start, Vector3 destination)
        {
            return Vector3.SignedAngle(start, destination, Vector3.up);
        }
    }
}