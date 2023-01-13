using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EndlessJumper.Scripts.Tower.SettingsData
{
    [CreateAssetMenu(fileName = "FloorSettings", menuName = "Tower/Floor Settings", order = 0)]
    public class FloorSettings : ScriptableObject
    {
        [SerializeField]
        private float MinFloorWidth;

        [SerializeField]
        private float MaxFloorWidth;

        [SerializeField]
        private float MinXPosition;

        [SerializeField]
        private float MaxXPosition;

        public (float width, float xPosition) GetRandomWidthAndPosition()
        {
            (float width, float xPosition) randomWidthAndPosition;
            var totalLevelWidth = MaxXPosition - MinXPosition;

            randomWidthAndPosition.width = Math.Clamp(Random.Range(MinFloorWidth, MaxFloorWidth), 0, totalLevelWidth);
            var center = randomWidthAndPosition.width / 2f;
            randomWidthAndPosition.xPosition = Random.Range(MinXPosition + center, MaxXPosition - center);
            return randomWidthAndPosition;
        }
    }
}