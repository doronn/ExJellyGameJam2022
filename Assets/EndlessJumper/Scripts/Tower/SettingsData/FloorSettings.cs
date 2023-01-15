using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace EndlessJumper.Scripts.Tower.SettingsData
{
    [CreateAssetMenu(fileName = "FloorSettings", menuName = "Tower/Floor Settings", order = 0)]
    public class FloorSettings : ScriptableObject
    {
        [SerializeField]
        private float MinFloorWidthChangePercentPerGeneration;
        [SerializeField]
        private float MaxFloorWidthChangePercentPerGeneration;
        
        [SerializeField]
        private float AbsoluteMinFloorWidth;
        
        [SerializeField]
        private float MinFloorWidth;

        [SerializeField]
        private float MaxFloorWidth;

        [field: SerializeField]
        public float MinXPosition { get; private set; }

        [field: SerializeField]
        public float MaxXPosition { get; private set; }
        
        [field: SerializeField]
        public float ChanceOfFullBar { get; private set; }
        
        public (float width, float xPosition) GetRandomWidthAndPosition()
        {
            (float width, float xPosition) randomWidthAndPosition;
            var totalLevelWidth = MaxXPosition - MinXPosition;

            if (Random.value < ChanceOfFullBar)
            {
                randomWidthAndPosition.width = totalLevelWidth;
                randomWidthAndPosition.xPosition = MinXPosition + totalLevelWidth / 2f;
            }
            else
            {
                randomWidthAndPosition.width =
                    Math.Clamp(Random.Range(MinFloorWidth, MaxFloorWidth), 0, totalLevelWidth);
                var center = randomWidthAndPosition.width / 2f;
                randomWidthAndPosition.xPosition = Random.Range(MinXPosition + center, MaxXPosition - center);
            }

            return randomWidthAndPosition;
        }

        public void ProgressFloorsGeneration()
        {
            MinFloorWidth -= MinFloorWidthChangePercentPerGeneration;
            if (MinFloorWidth < AbsoluteMinFloorWidth)
            {
                MinFloorWidth = AbsoluteMinFloorWidth;
            }
            
            MaxFloorWidth -= MaxFloorWidthChangePercentPerGeneration;
            if (MaxFloorWidth < MinFloorWidth)
            {
                MaxFloorWidth = MinFloorWidth;
            }
            if (MaxFloorWidth < AbsoluteMinFloorWidth)
            {
                MaxFloorWidth = AbsoluteMinFloorWidth;
            }
        }
    }
}