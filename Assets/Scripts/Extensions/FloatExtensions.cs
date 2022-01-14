namespace Assets.Scripts.Extensions
{
    public static class FloatExtensions
    {
        public static float Map(this float value, float oldFrom, float oldTo, float newFrom, float newTo)
        {
            return (value - oldFrom) / (oldTo - oldFrom) * (newTo - newFrom) + newFrom;
        }
    }
}
