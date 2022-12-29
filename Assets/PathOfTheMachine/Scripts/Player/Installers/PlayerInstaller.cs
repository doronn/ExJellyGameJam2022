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
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GameStartSignal>();
            Container.BindInstance(_playerProperties).AsSingle().NonLazy();
            // Container.BindInterfacesTo<PlayerController>().AsTransient();
            Container.BindInterfacesTo<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<IPlayerControllerFactory>().To<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<IReadPlayerValuesFactory>().To<PlayerControllerFactory>().AsCached().NonLazy();
            // Container.Bind<BotController>().AsTransient().NonLazy();
        }
    }
}