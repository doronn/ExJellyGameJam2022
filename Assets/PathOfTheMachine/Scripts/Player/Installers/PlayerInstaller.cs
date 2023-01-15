using PathOfTheMachine.Scripts.Player.Factories;
using PathOfTheMachine.Scripts.Signals;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Player.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerProperties _playerProperties;
        [SerializeField]
        private Transform _playerParent;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GameStartSignal>();
            var playerProperties = Instantiate(_playerProperties);
            Container.BindInstance(playerProperties).AsSingle().NonLazy();
            Container.BindInstance(_playerParent).WithId("parent").AsSingle().NonLazy();
            // Container.BindInterfacesTo<PlayerController>().AsTransient();
            Container.BindInterfacesTo<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<IPlayerControllerFactory>().To<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<IReadPlayerValuesFactory>().To<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<BotController>().AsTransient().NonLazy();
        }
    }
}