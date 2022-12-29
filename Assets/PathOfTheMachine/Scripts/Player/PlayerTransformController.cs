using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Player
{
    public class PlayerTransformController : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerTransform;

        [SerializeField]
        private float _movementInterpolationSpeed;

        private IReadPlayerValues _readPlayerValues;

        [Inject]
        public void Inject(IReadPlayerValues readPlayerValues)
        {
            _readPlayerValues = readPlayerValues;
        }

        private void Update()
        {
            var currentTransformPosition = _playerTransform.localPosition;
            currentTransformPosition = Vector3.Lerp(currentTransformPosition, _readPlayerValues.CurrentPlayerLocalPosition, _movementInterpolationSpeed * Time.deltaTime);
            _playerTransform.localPosition = currentTransformPosition;
        }
    }
}