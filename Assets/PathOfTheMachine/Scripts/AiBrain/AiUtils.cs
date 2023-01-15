using System;

namespace AiBrain
{
    public static class AiUtils
    {
        public static float GetNextRandom(this Random random)
        {
            return (float)(random.NextDouble() * 2 - 1);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}