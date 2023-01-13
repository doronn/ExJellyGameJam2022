using PathOfTheMachine.Scripts.Player.Factories;
using Scripts.Player.Platformer;
using UnityEngine;
using Zenject;

namespace PathOfTheMachine.Scripts.Player.ViewControllers
{
    public class PlayerTransformController : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerTransform;

        [SerializeField]
        private int _id = 0; 

        [SerializeField]
        private float _movementInterpolationSpeed;

        [SerializeField]
        private Vector3 _positionOffset = Vector3.zero;

        private IReadPlayerValues _readPlayerValues;

        [Inject]
        public void Inject(IReadPlayerValuesFactory readPlayerValuesFactory)
        {
            _readPlayerValues = readPlayerValuesFactory.Create(_id == 0 ? 0 : gameObject.GetInstanceID());
        }

        private void Update()
        {
            // var currentTransformPosition = _playerTransform.localPosition;
            // currentTransformPosition = Vector3.Lerp(currentTransformPosition, _readPlayerValues.CurrentPlayerLocalPosition + _positionOffset, _movementInterpolationSpeed * Time.deltaTime);
            _playerTransform.position = _readPlayerValues.CurrentPlayerLocalPosition + _positionOffset;
        }
    }
}