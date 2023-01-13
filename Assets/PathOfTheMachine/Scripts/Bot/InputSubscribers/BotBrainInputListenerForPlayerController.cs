using AiBrain;
using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Bot.InputSubscribers
{
    public class BotBrainInputListenerForPlayerController : MonoBehaviour, IBrainInputListener
    {
        [SerializeField]
        private Brain _brain;

        public int InputAmount => 3;
        
        private IPlayerController _botController;
        private BrainsManager _brainsManager;

        private float _horizontalInputAmount = 0;
        private bool _holdingJump = false;
        private bool _holdingJumpLastInput = false;
        private bool _jumpRegistered = false;

        private bool _horizontalInputRequested = false;

        [Inject]
        private void Inject(IPlayerControllerFactory botControllerFactory, BrainsManager brainsManager)
        {
            _botController = botControllerFactory.Create(gameObject.GetInstanceID());
            _brainsManager = brainsManager;
        }

        private void Start()
        {
            _brainsManager.RegisterBrain(_brain);
            _botController.ConnectController();
        }
        
        public bool DidReceiveBadInputs()
        {
            var badJumpInput = _holdingJump && !_jumpRegistered;
            var badHorizontalInput = _horizontalInputRequested && _horizontalInputAmount == 0;

            return badJumpInput || badHorizontalInput;
        }

        public void InputReset()
        {
            _horizontalInputAmount = 0;
            if (!_holdingJumpLastInput)
            {
                _holdingJump = false;
            }

            _holdingJumpLastInput = false;
            _jumpRegistered = false;
        }
        
        public void RegisterInput(int inputType, float inputValue)
        {
            switch (inputType)
            {
                case 0:
                {
                    if (inputValue > 0)
                    {
                        _holdingJumpLastInput = true;
                        if (!_holdingJump)
                        {
                            _holdingJump = true;
                            _jumpRegistered = true;
                        }
                    }
                    break;
                }
                case 1:
                {
                    _horizontalInputRequested = true;
                    _horizontalInputAmount += 1;
                    break;
                }
                case 2:
                {
                    _horizontalInputRequested = true;
                    _horizontalInputAmount -= 1;
                    break;
                }
                case 3:
                {
                    // _controlledCharacter.StrafeRight(); (spin sword to right)
                    break;
                }
                case 4:
                {
                    // _controlledCharacter.StrafeRight(); (spin sword to left)
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        
        public void ExecuteInputs()
        {
            if (_jumpRegistered)
            {
                _botController.RequestJump();
            }
            
            _botController.SetHorizontalInput(_horizontalInputAmount);
        }

        // Uncomment for random movement
        /*private void FixedUpdate()
        {
            switch (Random.Range(0, 5000))
            {
                case 0:
                    _botController.RequestJump();
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    _botController.SetHorizontalInput(-1);
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    _botController.SetHorizontalInput(1);
                    break;
                default:
                    _botController.SetHorizontalInput(0);
                    break;
            }
        }*/
    }
}