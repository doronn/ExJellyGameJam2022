using System.Collections.Generic;
using EndlessJumper.Scripts.Tower.Interfaces;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _objectPrefab;

        [SerializeField]
        private int _initialPoolSize;

        [field: SerializeField] public PoolObjectType PoolType { get; private set; }

        private Queue<Transform> _pool;
        
        private void Start()
        {
            _pool = new Queue<Transform>(_initialPoolSize);
            for (var i = 0; i < _initialPoolSize; i++)
            {
                var objectInstance = Instantiate(_objectPrefab);
                _pool.Enqueue(objectInstance);
                objectInstance.gameObject.SetActive(false);
            }
        }

        public void CreateObject(IPoolableObject poolableObject, Transform parentSegmentTransform = null)
        {
            if (!_pool.TryDequeue(out var poolObject))
            {
                poolObject = Instantiate(_objectPrefab);
            }
            poolObject.gameObject.SetActive(true);
            poolObject.SetParent(parentSegmentTransform);

            poolableObject.OnAdded(poolObject);
        }

        public void DestroyObject(IPoolableObject poolableObject)
        {
            var objectToPool = poolableObject.ObjectInstance;
            objectToPool.gameObject.SetActive(false);
            
            _pool.Enqueue(objectToPool);
            poolableObject.OnRemoved();
        }

        private void OnDestroy()
        {
            _pool.Clear();
        }
    }
}