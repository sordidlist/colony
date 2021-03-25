namespace _Custom.Code
{
    public class GrahamAI : CharacterAI
    {
        public void Start()
        {
            base.Start();
            characterProperties.HUNGER = 10;
            characterProperties.THIRST = 30;
            characterProperties.TIRED = 45;
            characterProperties.ALERT = 35;
            characterProperties.HAPPY = 65;
        }
    }
}