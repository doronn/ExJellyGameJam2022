using EndlessJumper.Scripts.Tower.Concrete;
using EndlessJumper.Scripts.Tower.Interfaces;
using EndlessJumper.Scripts.Tower.SettingsData;
using UnityEngine;
using Zenject;

namespace EndlessJumper.Scripts.Tower.Installers
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelsSettings _levelsSettings;

        [SerializeField]
        private FloorSettings _floorSettings;

        [SerializeField]
        private ObjectPoolFactory _objectPoolFactory;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_levelsSettings).AsSingle().NonLazy();
            Container.BindInstance(_floorSettings).AsSingle().NonLazy();
            Container.BindInterfacesTo<ObjectPoolFactory>().FromInstance(_objectPoolFactory).AsSingle().NonLazy();
            Container.BindInterfacesTo<PoolableObjectFactory>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LevelSegmentControllerFactory>().AsSingle().NonLazy();
            Container.BindInterfacesTo<FloorDataFactory>().AsSingle().NonLazy();
        }
    }
}