using System;
using AiBrain;
using PathOfTheMachine.Scripts.GameWorld;
using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using Sensory;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Bot.SensoryProviders
{
    public class BotSensoryProvider : MonoBehaviour, IGetSensoryData
    {
        public float[] Current { get; } = new float[18];
        public float Score { get; private set; }
        public bool Alive { get; } = true;

        private IReadPlayerValues _readPlayerValues;
        private IReadPlayerValues _readMyBotValues;
        private PlayerProperties _playerProperties;
        private WorldConfiguration _worldConfiguration;

        private float _maxPlayersDistance;
        private Ray _ray;
        private RaycastHit _hitInfo;

        private static readonly (Vector2 direction, bool toGround)[] RayDirections = {
                                                                          (Vector2.down, true),
                                                                          (Vector3.down + Vector3.right, true),
                                                                          (Vector3.right, false),
                                                                          (Vector3.right + Vector3.up, false),
                                                                          (Vector3.up, false),
                                                                          (Vector3.up + Vector3.left, false),
                                                                          (Vector3.left, false),
                                                                          (Vector3.left + Vector3.down, true)
                                                                      };

        [Inject]
        private void Inject(IReadPlayerValuesFactory readPlayerControllerFactory, PlayerProperties playerProperties, WorldConfiguration worldConfiguration)
        {
            _readPlayerValues = readPlayerControllerFactory.Create(0);
            _readMyBotValues = readPlayerControllerFactory.Create(gameObject.GetInstanceID());
            _playerProperties = playerProperties;
            _worldConfiguration = worldConfiguration;
        }

        private void Start()
        {
            _maxPlayersDistance = 0;
        }

        private void Update()
        {
            var currentPlayerLocalPosition = _readPlayerValues.CurrentPlayerLocalPosition;
            var currentBotLocalPosition = _readMyBotValues.CurrentPlayerLocalPosition;
            var currentPlayerDistance = Vector2.Distance(currentBotLocalPosition,
                currentPlayerLocalPosition);
            if (_maxPlayersDistance < currentPlayerDistance)
            {
                _maxPlayersDistance = currentPlayerDistance > 0 ? currentPlayerDistance : 1f;
            }
            // var currentPlayerDistanceSensor = currentPlayerDistance > 0 ? 1 : 0;
            // Current[0] = currentPlayerDistanceSensor;

            var playerDirection = (currentPlayerLocalPosition - currentBotLocalPosition).normalized;
            Current[0] = playerDirection.x;
            
            /*var xDistance = Math.Abs(currentPlayerLocalPosition.x - currentBotLocalPosition.x);
            Current[2] = xDistance;*/
            
            Current[1] = playerDirection.y;
        
            // var yDistance = Math.Abs(currentPlayerLocalPosition.y - currentBotLocalPosition.y);
            // Current[4] = yDistance;

            _ray.origin = currentBotLocalPosition;
            var i = 0;
            for (; i < RayDirections.Length; i++)
            {
                var rayDirection = RayDirections[i];
                _ray.direction = rayDirection.direction;
                Current[2 + i] = CalculateRayCollision(true);
            }

            for (var j = 0; j < RayDirections.Length; j++, i++)
            {
                var rayDirection = RayDirections[j];
                _ray.direction = rayDirection.direction;
                Current[2 + i] = CalculateRayCollision(false);
            }
            

            Score = currentPlayerDistance;
            var characterSizeX = _playerProperties.CharacterSize.x;
            _ray.direction = playerDirection;
            // if (currentPlayerDistance > 1/* && currentPlayerDistance < characterSizeX * 20*/)
            // {
                /*_ray.Draw(Color.magenta, currentPlayerDistance);
                Score += (_maxPlayersDistance - currentPlayerDistance + 1) * _maxPlayersDistance /
                         (currentPlayerDistance + 1);

                if (playerDirection.x is > 0.85f or < -0.85f)
                {
                    Score += _maxPlayersDistance * (1 - playerDirection.y);
                }*/
            // }
            // else
            // {
                // _ray.Draw(Color.yellow, currentPlayerDistance);
            // }
        }

        private float CalculateRayCollision(bool isAnyGround)
        {
            var checkDistance = Math.Abs(_ray.direction.x * _playerProperties.CharacterSize.x) + Math.Abs(_ray.direction.y * _playerProperties.CharacterSize.y);
            // var checkDistance = Math.Abs(Vector3.Dot(_ray.direction, _playerProperties.CharacterSize));
            var groundCheckDistance = checkDistance * 1.5f;
            if (Physics.Raycast(_ray, out _hitInfo, groundCheckDistance, isAnyGround ? _playerProperties.groundLayer : _playerProperties.solidGroundLayer))
            {
                _ray.Draw((isAnyGround ? Color.blue : Color.green), _hitInfo.distance);
                return Math.Clamp(_hitInfo.distance, 0, groundCheckDistance) / groundCheckDistance;
            }

            _ray.Draw(Color.red, groundCheckDistance);

            return 1;
        }
    }
}