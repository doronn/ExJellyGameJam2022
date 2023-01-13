using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.GameWorld
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField]
        private WorldConfiguration _worldConfiguration;

        [SerializeField]
        private Transform _worldParent;
        
        private EndlessJumperWorldLoader _worldLoader;
        // private WorldLoader _worldLoader;
        
        public override void InstallBindings()
        {
            Container.Bind<WorldConfiguration>().FromInstance(_worldConfiguration).AsSingle().NonLazy();
            // Container.Bind<WorldLoader>().AsSingle().NonLazy();
            Container.Bind<EndlessJumperWorldLoader>().AsSingle().NonLazy();
            
            
        }

        public override void Start()
        {
            if (_worldLoader == null)
            {
                _worldLoader = Container.Instantiate<EndlessJumperWorldLoader>();
                // _worldLoader = Container.Instantiate<WorldLoader>();
            }
            _worldLoader.InitWorld(_worldParent);
        }
    }
}