
    
namespace _Custom.Code.Creature_System.Interfaces
{
    public interface IAnimationHandler
    {
        public void SetInstanceMovementSpeed(CreatureAgent.CreatureAgent instance);
        public void SetRotateSpeed(CreatureAgent.CreatureAgent instance);
        public void TriggerInstanceBite(CreatureAgent.CreatureAgent instance);
        public void TriggerInstanceAttack(CreatureAgent.CreatureAgent instance);
        public void TriggerInstanceDeath(CreatureAgent.CreatureAgent instance);
    }
}