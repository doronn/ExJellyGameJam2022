using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace PathOfTheMachine.Scripts.Player.PlayerInputSubscribers
{
    public class PlayerInputListenerForPlayerController : MonoBehaviour
    {
        private IPlayerController _playerController;

        [Inject]
        private void Inject(IPlayerControllerFactory playerControllerFactory)
        {
            _playerController = playerControllerFactory.Create(0);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton())
            {
                return;
            }
            
            _playerController.RequestJump();
        }
        
        public void HorizontalMovement(InputAction.CallbackContext context)
        {
            _playerController.SetHorizontalInput(context.ReadValue<float>());
        }
    }
}