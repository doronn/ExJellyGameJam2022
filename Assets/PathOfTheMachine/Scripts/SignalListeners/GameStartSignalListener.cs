using PathOfTheMachine.Scripts.Player.Factories;
using PathOfTheMachine.Scripts.Signals;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.SignalListeners
{
    public class GameStartSignalListener : MonoBehaviour
    {
        private IPlayerController _playerController;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Inject(SignalBus signalBus, IPlayerControllerFactory playerControllerFactory)
        {
            _signalBus = signalBus;
            _playerController = playerControllerFactory.Create(0);
        }

        private void Awake()
        {
            _signalBus.Subscribe<GameStartSignal>(GameStarted);
        }

        private void GameStarted()
        {
            _playerController.ConnectController();;
        }
    }
}