using System.Collections.Generic;
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

        [Inject]
        public PlayerControllerFactory(DiContainer container, PlayerProperties playerProperties)
        {
            _container = container;
            _playerProperties = playerProperties;
            _playerControllerCache = new Dictionary<int, PlayerController>();
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
                playerController.Inject(_playerProperties);
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