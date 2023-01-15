using UnityEngine;

namespace Sensory
{
    public static class RayExtensions
    {
        public static void Draw(this Ray ray, float length = 1f)
        {
            ray.Draw(Color.green, 0f, length);
        }
        
        public static void Draw(this Ray ray, Color color, float length = 1f)
        {
            ray.Draw(color, 0f, length);
        }
        
        public static void Draw(this Ray ray, Color color, float duration, float length)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * length, color, duration, false);
        }
    }
}