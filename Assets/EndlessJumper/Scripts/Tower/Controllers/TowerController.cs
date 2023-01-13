using System;
using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using GameJamKit.Scripts.Utils.Attributes;
using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace EndlessJumper.Scripts.Tower.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private LevelsSettings _levelsSettings;
        private IPoolableObject[] _poolableObjects;
        private IPoolableObject[] _rotationArray = new IPoolableObject[1];

        private ILevelSegmentControllerFactory _levelSegmentControllerFactory;
        private ObjectPoolManager _levelSegmentPoolManager;
        private float _totalVerticalOffset;
        private Transform _holderTransform;
        private IPlayerController _playerController;
        private float _currentVerticalMovementSpeed = 0f;

        [Inject]
        public void Init(LevelsSettings levelsSettings, IObjectPoolFactory objectPoolFactory, ILevelSegmentControllerFactory levelSegmentControllerFactory, IPoolableObjectFactory poolableObjectFactory, IPlayerControllerFactory playerControllerFactory)
        {
            _levelsSettings = levelsSettings;
            _levelSegmentPoolManager = objectPoolFactory.Create(PoolObjectType.LevelSegment);
            _levelSegmentControllerFactory = levelSegmentControllerFactory;
            _playerController = playerControllerFactory.Create(0);
            _currentVerticalMovementSpeed = _levelsSettings.VerticalMovementSpeed;
        }
        
        private void Start()
        {
            _holderTransform = transform;
            _poolableObjects = new IPoolableObject[_levelsSettings.LevelSegmentsCount];
            for (var i = 0; i < _levelsSettings.LevelSegmentsCount; i++)
            {
                var currentObject = _poolableObjects[i] = _levelSegmentControllerFactory.Create();
                _levelSegmentPoolManager.CreateObject(currentObject, _holderTransform);
                currentObject.ObjectInstance.position =
                    Vector3.up * ((i - _levelsSettings.LevelSegmentsCount / 2) * _levelsSettings.LevelSegmentsHeight);
            }
            
            _playerController.SetConstantVerticalSpeed(_currentVerticalMovementSpeed);
        }

        private void Update()
        {
            _totalVerticalOffset += _currentVerticalMovementSpeed * Time.deltaTime;
            
            if (_totalVerticalOffset >= _levelsSettings.LevelSegmentsHeight)
            {
                var poolableObjectsLength = _poolableObjects.Length;
                _totalVerticalOffset -= _levelsSettings.LevelSegmentsHeight;
                var lowestLevelSegment = _poolableObjects[0];
                var currentObjectInstance = lowestLevelSegment.ObjectInstance;
                lowestLevelSegment.OnRemoved();
                lowestLevelSegment.OnAdded(currentObjectInstance);

                _rotationArray[0] = lowestLevelSegment;
                Array.Copy(_poolableObjects, 1, _poolableObjects, 0, poolableObjectsLength - 1);
                _poolableObjects[poolableObjectsLength - 1] = _rotationArray[0];
                
                for (var i = 0; i < poolableObjectsLength; i++)
                {
                    var currentObject = _poolableObjects[i];
                    currentObject.ObjectInstance.localPosition =
                        Vector3.up *
                        ((i - _levelsSettings.LevelSegmentsCount / 2) * _levelsSettings.LevelSegmentsHeight);
                }
            }
            
            _holderTransform.localPosition = Vector3.up * -_totalVerticalOffset;
        }

        [Button]
        private void EnhanceConstantVelocity()
        {
            _currentVerticalMovementSpeed += 0.5f;
            _playerController.SetConstantVerticalSpeed(_currentVerticalMovementSpeed);
        }
    }
}