using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;

namespace EndlessJumper.Scripts.Tower.Concrete
{
    public class FloorDataFactory : IFloorDataFactory
    {
        private readonly FloorSettings _floorSettings;

        public FloorDataFactory(FloorSettings floorSettings)
        {
            _floorSettings = floorSettings;
        }

        public IFloorData Create()
        {
            return new FloorData(_floorSettings);
        }
    }
}