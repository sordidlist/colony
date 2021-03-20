namespace _Custom.Code.Creature_System.Interfaces
{
    public interface NavigationHandler
    {
        public bool HasDestination();
        public void SetDestination();
        public void GetDistanceEuclidian();
        public void GetDestinationAngle();
    }
}