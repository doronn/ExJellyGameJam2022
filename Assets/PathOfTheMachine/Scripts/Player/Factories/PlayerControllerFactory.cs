using System.Collections.Generic;
using System.Linq;
using PathOfTheMachine.Scripts.GameWorld;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Player.Factories
{
    public class PlayerControllerFactory : IReadPlayerValuesFactory, IPlayerControllerFactory
    {
        private readonly DiContainer _container;
        private readonly PlayerProperties _playerProperties;
        private readonly Dictionary<int, PlayerController> _playerControllerCache;
        private readonly Vector2 _playerStartPosition;
        private readonly Vector2 _botStartPosition;

        [Inject(Id = "parent")]
        private Transform _playerParent;
        
        [Inject]
        public PlayerControllerFactory(DiContainer container, PlayerProperties playerProperties, WorldConfiguration worldConfiguration)
        {
            _container = container;
            _playerProperties = playerProperties;
            _playerControllerCache = new Dictionary<int, PlayerController>();
            var locations = worldConfiguration.StartLocations.ToList();
            var nextLocationIndex = Random.Range(0, locations.Count);
            _playerStartPosition = locations[nextLocationIndex];
            locations.RemoveAt(nextLocationIndex);

            if (locations.Count > 0)
            {
                nextLocationIndex = Random.Range(0, locations.Count);
                _botStartPosition = locations[nextLocationIndex];
            }
        }

        IPlayerController IPlayerControllerFactory.Create(int id)
        {
            return Create(id);
        }

        IReadPlayerValues IReadPlayerValuesFactory.Create(int id)
        {
            return Create(id);
        }
        
        public PlayerController Create(int id)
        {
            if (_playerControllerCache.ContainsKey(id))
            {
                return _playerControllerCache[id];
            }
            else
            {
                // Create and return a new instance of PlayerController
                var playerController = new GameObject($"PlayerController {id}").AddComponent<PlayerController>();
                // playerController.transform.SetParent(_playerParent);
                playerController.Inject(_playerProperties, id, id == 0 ? _playerStartPosition : _botStartPosition);
                _playerControllerCache[id] = playerController;
                return playerController;
            }
        }
    }

    public interface IPlayerControllerFactory
    {
        IPlayerController Create(int id);
    }

    public interface IReadPlayerValuesFactory
    {
        IReadPlayerValues Create(int id);
    }
}