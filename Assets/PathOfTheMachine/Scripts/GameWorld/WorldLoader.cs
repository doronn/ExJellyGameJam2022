using Cysharp.Threading.Tasks;
using GameJamKit.Scripts.Utils.Attributes;
using PathOfTheMachine.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.GameWorld
{
    public class WorldLoader
    {
        private DiContainer _container;
        private readonly WorldConfiguration _worldConfiguration;
        
        private SignalBus _signalBus;
        
        [Inject]
        public WorldLoader(WorldConfiguration worldConfiguration, SignalBus signalBus, DiContainer diContainer)
        {
            _worldConfiguration = worldConfiguration;
            _signalBus = signalBus;
            _container = diContainer;
        }
        
        [Button]
        public void InitWorld()
        {
            InitWorldAsync().Forget();
        }

        private async UniTaskVoid InitWorldAsync()
        {
            var worldWidth = _worldConfiguration.WorldSize.x;
            var worldHeight = _worldConfiguration.WorldSize.y;
            
            var levelWidth = _worldConfiguration.LevelSize.x;
            var levelHeight = _worldConfiguration.LevelSize.y;
            
            var levelPrefab = _worldConfiguration.LevelPrefab;
            var playerPrefab = _worldConfiguration.PlayerPrefab;
            var botPrefab = _worldConfiguration.BotPrefab;

            for (var i = 0; i < worldWidth; i++)
            {
                for (var j = 0; j < worldHeight; j++)
                {
                    var nextLevelPosition = new Vector3(i * levelWidth, j * levelHeight);
                    var levelInstance = Object.Instantiate(levelPrefab, nextLevelPosition, Quaternion.identity);
                    _container.InstantiatePrefab(playerPrefab, levelInstance.transform);
                    _container.InstantiatePrefab(botPrefab, levelInstance.transform);
                    if (i == 0 && j == 0)
                    {
                        _signalBus.Fire<GameStartSignal>();
                    }
                    await UniTask.DelayFrame(1);
                }
            }
        }
    }
}