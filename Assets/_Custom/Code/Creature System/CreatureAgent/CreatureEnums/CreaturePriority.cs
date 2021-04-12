namespace _Custom.Code.Creature_System.CreatureAgent
{
    public enum CreaturePriority
    {
        GATHER_FOOD,
        EXPAND_COLONY,
        ESCAPE_THREAT,
        PROTECT_COLONY,
    }

    public enum CreaturePriorityValue
    {
        URGENT,
        HIGH,
        AVERAGE,
        LOW
    }

    public class CreaturePrioritySetting
    {
        public CreaturePrioritySetting(CreaturePriority priority, CreaturePriorityValue value)
        {
            creaturePriority = priority;
            creaturePriorityValue = value;
        }
        
        public CreaturePriority GetPriority()
        {
            return creaturePriority;
        }

        public CreaturePriorityValue GetValue()
        {
            return creaturePriorityValue;
        }
        
        private CreaturePriority creaturePriority;
        private CreaturePriorityValue creaturePriorityValue;
    }
}