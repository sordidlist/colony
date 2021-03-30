namespace _Custom.Code.Creature_System
{
    public class StickyHandler : Singleton<StickyHandler>
    {
        private SensorRaycastHandler sensorRaycastHandler;
        
        public void CheckStickiness()
        {
            
        }

        public void EnableStickyInstance()
        {
            
        }

        public void DisableStickyInstance()
        {
            
        }

        public void SetSensorRaycastHandler(SensorRaycastHandler sensorRaycastHandler)
        {
            this.sensorRaycastHandler = sensorRaycastHandler;
        }
    }
}