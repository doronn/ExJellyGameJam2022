using System;
using UnityEngine;

namespace Sensory
{/*
    public class DiscreteSensor : MonoBehaviour, ISensor, ISensoryMetaData
    {
        [SerializeField]
        private int _sensorsCount;

        [SerializeField, Range(0, 1)]
        private float _sensorSpread;

        [SerializeField]
        private float _sensorRange;
        
        [SerializeField]
        private float _sensorStartDistance;
        
        [SerializeField]
        private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.Collide;
        
        [SerializeField]
        private LayerMask _collisionLayerMask = ~0;
        
        [SerializeField]
        private Color _gizmoColor = Color.red;

        [SerializeField]
        private float _sphereCastRadius = 0.3f;

        public int OwnerId { get; private set; }
        public int SensorCount => _sensorsCount;
        private float SensorMaxDistance => _sensorRange - _sensorStartDistance;
        
        private ISensorListener _sensorListener;
        private Ray _sensorRay;
        private RaycastHit _raycastHit;
        private int _forwardSensorIndex;
        private bool _anySensorHit;
        private RaycastHit[] _raycastHitsBuffer = new RaycastHit[15];

        public void RegisterListener(ISensorListener sensorListener, int ownerId)
        {
            OwnerId = ownerId;
            _sensorListener = sensorListener;
            sensorListener.Init(_sensorsCount, this);
        }

        float[] ISensoryMetaData.OnRequestSensoryData(float[] arrayBuffer)
        {
            var forward = transform.forward;
            
            _anySensorHit = false;
            var arrayBufferLength = Math.Min(arrayBuffer.Length, _sensorsCount);
            for (var i = 0; i < arrayBufferLength; i++)
            {
                arrayBuffer[i] = 0;
                InitRay(i, arrayBufferLength, forward);
                var foundHits = Physics.SphereCastNonAlloc(_sensorRay, _sphereCastRadius, _raycastHitsBuffer, SensorMaxDistance, _collisionLayerMask, _queryTriggerInteraction);
                var minDistance = SensorMaxDistance + 1;
                for (var j = 0; j < foundHits; j++)
                {
                    _raycastHit = _raycastHitsBuffer[j];
                    if (_raycastHit.collider.gameObject.TryGetComponent<IIdableActor>(out var actor) && actor.Id == OwnerId)
                    {
                        continue;
                    }

                    _anySensorHit = true;
                    if (_raycastHit.distance < minDistance)
                    {
                        minDistance = _raycastHit.distance;
                        var distanceFactor = (actor.Id == -1 || actor is DamageGiver ? -1 : 1);
                        arrayBuffer[i] = (1 - _raycastHit.distance / SensorMaxDistance) * distanceFactor;
                    }
                }
            }

            return arrayBuffer;
        }

        int ISensoryMetaData.GetForwardSensorIndex() => _forwardSensorIndex; 
        bool ISensoryMetaData.GetAnySensorHit() => _anySensorHit; 

        private void InitRay(int i, int totalCount, Vector3 forward)
        {
            var spreadStep = ((i - totalCount / 2) / (float)totalCount);
            var rayDirection = Vector3.SlerpUnclamped(forward, -forward, _sensorSpread * spreadStep * 2);
            _sensorRay.direction = rayDirection;
            _sensorRay.origin = transform.position + rayDirection.normalized * _sensorStartDistance;

            if (spreadStep == 0)
            {
                _forwardSensorIndex = i;
            }
            // _sensorRay.Draw(_gizmoColor, SensorMaxDistance);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _gizmoColor;
            var forward = transform.forward;
            for (var i = 0; i < _sensorsCount; i++)
            {
                InitRay(i, _sensorsCount, forward);
                Gizmos.DrawLine(_sensorRay.origin, _sensorRay.origin + _sensorRay.direction * SensorMaxDistance);
            }
        }
    }*/
}