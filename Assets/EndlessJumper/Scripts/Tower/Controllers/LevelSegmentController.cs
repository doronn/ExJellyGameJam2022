using EndlessJumper.Scripts.Tower.Concrete;
using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Controllers
{
    public class LevelSegmentController : BasePoolableObject
    {
        private readonly ObjectPoolManager _objectPoolManager;
        private bool _initialized = false;
        private IFloorData[] _floors;
        private float _distanceBetweenFloors;

        public LevelSegmentController(LevelsSettings levelSettings, IObjectPoolFactory objectPoolFactory, IFloorDataFactory floorDataFactory)
        {
            _floors = new IFloorData[levelSettings.FloorsInSegment];
            _distanceBetweenFloors = levelSettings.LevelSegmentsHeight / levelSettings.FloorsInSegment;
            for (var i = 0; i < _floors.Length; i++)
            {
                _floors[i] = floorDataFactory.Create();
            }
            _objectPoolManager = objectPoolFactory.Create(PoolObjectType.Floor);
        }

        public override void OnAdded(Transform objectInstance)
        {
            base.OnAdded(objectInstance);
            InitLevelSegment();
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            DestroyLevelSegment();
        }

        private void InitLevelSegment()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            for (var i = 0; i < _floors.Length; i++)
            {
                _floors[i].Init(i * _distanceBetweenFloors);
                _objectPoolManager.CreateObject(_floors[i], ObjectInstance);
            }
        }

        private void DestroyLevelSegment()
        {
            if (!_initialized)
            {
                return;
            }

            for (var i = 0; i < _floors.Length; i++)
            {
                _objectPoolManager.DestroyObject(_floors[i]);
            }
            _initialized = false;
        }
    }
}