namespace _Custom.Code
{
    public class CharacterProperties
    {
        public float HUNGER;
        public float THIRST;
        public float TIRED;
        public float ALERT;
        public float HAPPY;
        public float MAX_PROPERTY_VALUE = 100;

        public float GetPropertyAsPercentage(float property)
        {
            return property / MAX_PROPERTY_VALUE;
        }
    }
}