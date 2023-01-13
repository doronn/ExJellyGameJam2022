using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Concrete
{
    public class FloorData : BasePoolableObject, IFloorData
    {
        private readonly FloorSettings _floorSettings;

        public FloorData(FloorSettings floorSettings)
        {
            _floorSettings = floorSettings;
        }

        public override void OnAdded(Transform objectInstance)
        { 
            base.OnAdded(objectInstance);
            var scaleVector = Vector3.one;
            
            scaleVector.x = Width;
            ObjectInstance.localScale = scaleVector;
            
            ObjectInstance.localPosition = new Vector2(X, Y);
        }

        public float Width { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public void Init(float yPosition)
        {
            Y = yPosition;
            (Width, X) = _floorSettings.GetRandomWidthAndPosition();
        }
    }
}