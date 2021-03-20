namespace _Custom.Code.Creature_System.Interfaces
{
    public interface IPhereomoneHandler
    {
        public void DetectFoodPheromone();
        public void DetectFearPheromone();
        public void PlaceFoodPheromone();
        public void PlaceFearPheromone();
    }
}