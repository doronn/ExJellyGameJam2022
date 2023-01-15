using Cysharp.Threading.Tasks;
using EndlessJumper.Scripts.Tower.Concrete;
using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Controllers
{
    public class LevelSegmentController : BasePoolableObject
    {
        private readonly FloorSettings _floorSettings;
        private readonly ObjectPoolManager _objectPoolManager;
        private readonly IFloorData[] _floors;
        private readonly float _distanceBetweenFloors;
        
        private int _initializing = 0;
        private int _deInitializing = 0;
        private bool _initialized = false;

        private bool _isFirstInitialization = true;

        public LevelSegmentController(LevelsSettings levelSettings, FloorSettings floorSettings, IObjectPoolFactory objectPoolFactory, IFloorDataFactory floorDataFactory)
        {
            _floorSettings = floorSettings;
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
            InitLevelSegment(_isFirstInitialization).Forget();
            _isFirstInitialization = false;
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            DestroyLevelSegment().Forget();
        }

        private async UniTaskVoid InitLevelSegment(bool isFirstInit)
        {
            if (_initialized && _deInitializing == 0 || _initializing > 0)
            {
                return;
            }

            var totalInitializationLength = _floors.Length;
            _initializing = totalInitializationLength;
            if (!isFirstInit)
            {
                await UniTask.DelayFrame(1);
                await UniTask.WaitUntil(CanContinueInitializePredicate);
            }

            for (var i = 0; i < totalInitializationLength; i++)
            {
                _floors[i].Init(i * _distanceBetweenFloors);
                _objectPoolManager.CreateObject(_floors[i], ObjectInstance);
                if (!isFirstInit)
                {
                    await UniTask.DelayFrame(1);
                    _initializing = totalInitializationLength - i;
                    await UniTask.WaitUntil(CanContinueInitializePredicate);
                }
            }
            
            _floorSettings.ProgressFloorsGeneration();
            _initializing = 0;
            _initialized = true;
        }

        private bool CanContinueInitializePredicate()
        {
            return _deInitializing < _initializing;
        }

        private async UniTaskVoid DestroyLevelSegment()
        {
            if (!_initialized && _initializing > 0)
            {
                await UniTask.WaitUntil(() => _initialized);
            }
            if (!_initialized || _deInitializing > 0)
            {
                return;
            }

            var totalDeInitializationLength = _floors.Length;
            _deInitializing = totalDeInitializationLength;

            await UniTask.DelayFrame(1);
            for (var i = 0; i < totalDeInitializationLength; i++)
            {
                _objectPoolManager.DestroyObject(_floors[i]);
                await UniTask.DelayFrame(1);
                _deInitializing = totalDeInitializationLength - i;
            }
            
            _deInitializing = 0;
            _initialized = false;
        }
    }
}