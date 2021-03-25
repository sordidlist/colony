namespace _Custom.Code
{
    public class LorraineAI : CharacterAI
    {
        public void Start()
        {
            base.Start();
            characterProperties.HUNGER = 10;
            characterProperties.THIRST = 40;
            characterProperties.TIRED = 55;
            characterProperties.ALERT = 65;
            characterProperties.HAPPY = 55;
        }
    }
}