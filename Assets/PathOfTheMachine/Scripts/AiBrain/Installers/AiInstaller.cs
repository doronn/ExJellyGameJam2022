using UnityEngine;
using Zenject;

namespace AiBrain.Installers
{
    public class AiInstaller : MonoInstaller
    {
        [SerializeField]
        private BrainsManager _brainsManagerInstance;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_brainsManagerInstance).AsSingle().NonLazy();
        }
    }
}