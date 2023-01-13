using System.Threading;
using AiBrain;
using Cysharp.Threading.Tasks;
using GameJamKit.Scripts.Utils.Attributes;
using PathOfTheMachine.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.GameWorld
{
    public class WorldLoader
    {
        private readonly WorldConfiguration _worldConfiguration;
        private readonly SignalBus _signalBus;
        private readonly DiContainer _container;
        private readonly BrainsManager _brainsManager;

        [Inject]
        public WorldLoader(WorldConfiguration worldConfiguration, SignalBus signalBus, DiContainer diContainer, BrainsManager brainsManager)
        {
            _worldConfiguration = worldConfiguration;
            _signalBus = signalBus;
            _container = diContainer;
            _brainsManager = brainsManager;
        }
        
        [Button]
        public void InitWorld()
        {
            var cancellationToken = _brainsManager.GetCancellationTokenOnDestroy();
            InitWorldAsync(cancellationToken).Forget();
        }

        private async UniTaskVoid InitWorldAsync(CancellationToken cancellationToken)
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
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    var nextLevelPosition = new Vector3(i * levelWidth, j * levelHeight);
                    var levelInstance = Object.Instantiate(levelPrefab, nextLevelPosition, Quaternion.identity);
                    _container.InstantiatePrefab(playerPrefab, levelInstance.transform);
                    _container.InstantiatePrefab(botPrefab, levelInstance.transform);
                    if (i == 0 && j == 0)
                    {
                        _signalBus.Fire<GameStartSignal>();
                    }
                    if (j % 5 == 0)
                    {
                        await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
                    }
                }
            }
            
            _brainsManager.StartSimulation().Forget();
        }
    }
}