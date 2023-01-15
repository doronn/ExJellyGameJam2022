using System;
using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using GameJamKit.Scripts.Utils.Attributes;
using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EndlessJumper.Scripts.Tower.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private LevelsSettings _levelsSettings;
        private FloorSettings _floorSettings;
        private IPoolableObject[] _poolableObjects;
        private IPoolableObject[] _rotationArray = new IPoolableObject[1];

        private ILevelSegmentControllerFactory _levelSegmentControllerFactory;
        private ObjectPoolManager _levelSegmentPoolManager;
        private float _totalVerticalOffset;
        private Transform _holderTransform;
        private IPlayerController _playerController;
        private IReadPlayerValues _playerReadValues;
        private float _currentVerticalMovementSpeed = 0f;
        private float _currentVerticalMovementSpeedForPlayer = 0f;

        private float _playerMovementDirection = 0;
        private float _percentageOutOfMaxSpeed = 0f;

        [Inject]
        public void Init(LevelsSettings levelsSettings, FloorSettings floorSettings, IObjectPoolFactory objectPoolFactory, ILevelSegmentControllerFactory levelSegmentControllerFactory, IPoolableObjectFactory poolableObjectFactory, IPlayerControllerFactory playerControllerFactory, IReadPlayerValuesFactory readPlayerValuesFactory)
        {
            _levelsSettings = levelsSettings;
            _floorSettings = floorSettings;
            _levelSegmentPoolManager = objectPoolFactory.Create(PoolObjectType.LevelSegment);
            _levelSegmentControllerFactory = levelSegmentControllerFactory;
            _playerController = playerControllerFactory.Create(0);
            _playerReadValues = readPlayerValuesFactory.Create(0);
            _currentVerticalMovementSpeed = _levelsSettings.VerticalMovementSpeed;
            _currentVerticalMovementSpeedForPlayer = _levelsSettings.VerticalMovementSpeed;
        }
        
        private void Start()
        {
            _playerMovementDirection = 1;
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

        private void FixedUpdate()
        {
            if (_currentVerticalMovementSpeed < _levelsSettings.MaximumVerticalMovementSpeed)
            {
                _currentVerticalMovementSpeed += _levelsSettings.SpeedupAmountPerSecond.Evaluate(_percentageOutOfMaxSpeed) * Time.fixedDeltaTime;
                _playerController.SetConstantVerticalSpeed(_currentVerticalMovementSpeedForPlayer);
                _percentageOutOfMaxSpeed = (_currentVerticalMovementSpeed /
                                            _levelsSettings.MaximumVerticalMovementSpeed);
            }
            
            var movementSpeedOfMaximumPercent = 1f + _percentageOutOfMaxSpeed * 1.25f;

            _playerController.SetJumpForcePercentage(movementSpeedOfMaximumPercent * 4f);
            _playerController.SetMovementSpeedPercentage(movementSpeedOfMaximumPercent * 8f);
        }

        private void Update()
        {
            var playerYPosition = _playerReadValues.CurrentPlayerLocalPosition.y;
            if (playerYPosition < _levelsSettings.DeathYValue)
            {
                SceneManager.LoadScene(0);
                return;
            }

            var playerXPosition = _playerReadValues.CurrentPlayerLocalPosition.x;
            if (_playerMovementDirection > 0 && _floorSettings.MaxXPosition - playerXPosition < 1f)
            {
                _playerMovementDirection = -1f;
            }
            else if (_playerMovementDirection < 0 && playerXPosition - _floorSettings.MinXPosition < 1f)
            {
                _playerMovementDirection = 1f;
            }
            
            _playerController.SetHorizontalInput(_playerMovementDirection);
            
            if (playerYPosition > _levelsSettings.CatchupYValue)
            {
                _currentVerticalMovementSpeedForPlayer = Mathf.Lerp(_currentVerticalMovementSpeedForPlayer, _currentVerticalMovementSpeedForPlayer + playerYPosition,
                    Time.deltaTime);
            }
            else
            {
                _currentVerticalMovementSpeedForPlayer = _currentVerticalMovementSpeed;
            }
            _totalVerticalOffset += _currentVerticalMovementSpeedForPlayer * Time.deltaTime;

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