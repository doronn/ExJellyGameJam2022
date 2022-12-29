using Scripts.Player.Platformer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PathOfTheMachine.Scripts.Player.PlayerInputSubscribers
{
    public class PlayerInputListenerForPlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;

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