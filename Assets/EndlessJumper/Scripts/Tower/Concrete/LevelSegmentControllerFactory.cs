using EndlessJumper.Scripts.Tower.Controllers;
using EndlessJumper.Scripts.Tower.SettingsData;

namespace EndlessJumper.Scripts.Tower.Interfaces
{
    public class LevelSegmentControllerFactory : ILevelSegmentControllerFactory
    {
        private readonly LevelsSettings _levelsSettings;
        private readonly IObjectPoolFactory _objectPoolFactory;
        private readonly IFloorDataFactory _floorDataFactory;

        public LevelSegmentControllerFactory(LevelsSettings levelsSettings, IObjectPoolFactory objectPoolFactory, IFloorDataFactory floorDataFactory)
        {
            _levelsSettings = levelsSettings;
            _objectPoolFactory = objectPoolFactory;
            _floorDataFactory = floorDataFactory;
        }

        public LevelSegmentController Create()
        {
            return new LevelSegmentController(_levelsSettings, _objectPoolFactory, _floorDataFactory);
        }
    }
}