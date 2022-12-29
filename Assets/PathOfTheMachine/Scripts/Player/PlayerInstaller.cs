using GameJamKit.Scripts.Utils.Attributes;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private GameObject _playerPrefab;
        public override void InstallBindings()
        {
            Container.Bind<IReadPlayerValues>().To<PlayerController>().FromInstance(_playerController).NonLazy();
        }

        [Button]
        private void AddPlayer()
        {
            Container.InstantiatePrefab(_playerPrefab);
        }
    }
}