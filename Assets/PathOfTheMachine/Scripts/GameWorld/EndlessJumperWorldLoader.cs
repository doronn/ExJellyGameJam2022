using Cysharp.Threading.Tasks;
using GameJamKit.Scripts.Utils.Attributes;
using PathOfTheMachine.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.GameWorld
{
    public class EndlessJumperWorldLoader
    {
        private readonly WorldConfiguration _worldConfiguration;
        private readonly SignalBus _signalBus;
        private readonly DiContainer _container;

        [Inject]
        public EndlessJumperWorldLoader(WorldConfiguration worldConfiguration, SignalBus signalBus, DiContainer diContainer)
        {
            _worldConfiguration = worldConfiguration;
            _signalBus = signalBus;
            _container = diContainer;
        }
        
        [Button]
        public void InitWorld(Transform worldParent)
        {
            InitWorldAsync(worldParent).Forget();
        }

        private async UniTaskVoid InitWorldAsync(Transform worldParent)
        {
            var playerPrefab = _worldConfiguration.PlayerPrefab;
            _container.InstantiatePrefab(playerPrefab);
            
            _signalBus.Fire<GameStartSignal>();
        }
    }
}