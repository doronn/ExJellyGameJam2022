using System;
using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PathOfTheMachine.Scripts.Bot.InputSubscribers
{
    public class BotBrainInputListenerForPlayerController : MonoBehaviour
    {
        private IPlayerController _botController;

        [Inject]
        private void Inject(IPlayerControllerFactory botControllerFactory)
        {
            _botController = botControllerFactory.Create(gameObject.GetInstanceID());
        }

        private void Start()
        {
            _botController.ConnectController();
        }

        private void FixedUpdate()
        {
            switch (Random.Range(0, 10000))
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
        }
    }
}