using GameJamKit.Scripts.Utils.Attributes;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.GameWorld
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField]
        private WorldConfiguration _worldConfiguration;
        
        private WorldLoader _worldLoader;
        
        public override void InstallBindings()
        {
            Container.Bind<WorldConfiguration>().FromInstance(_worldConfiguration).AsSingle().NonLazy();
            Container.Bind<WorldLoader>().AsSingle().NonLazy();
        }

        [Button]
        public void TestWorldLoad()
        {
            if (_worldLoader == null)
            {
                _worldLoader = Container.Instantiate<WorldLoader>();
            }
            _worldLoader.InitWorld();
        }
    }
}