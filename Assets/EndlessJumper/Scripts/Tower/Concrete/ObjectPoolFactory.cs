using System.Collections.Generic;
using EndlessJumper.Scripts.Tower.Interfaces;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Concrete
{
    public class ObjectPoolFactory : MonoBehaviour, IObjectPoolFactory
    {
        private readonly Dictionary<PoolObjectType, ObjectPoolManager> _objectPoolManagersMap = new();

        [SerializeField]
        private ObjectPoolManager[] _objectPoolManagers;

        public ObjectPoolManager Create(PoolObjectType objectType)
        {
            if (_objectPoolManagersMap.TryGetValue(objectType, out var objectPoolManager))
            {
                return objectPoolManager;
            }

            var poolManagersCount = _objectPoolManagers.Length;
            for (int i = 0; i < poolManagersCount; i++)
            {
                if (_objectPoolManagers[i].PoolType == objectType)
                {
                    _objectPoolManagersMap[objectType] = _objectPoolManagers[i];
                    return _objectPoolManagers[i];
                }
            }

            Debug.LogException(new MissingReferenceException(
                $"Couldn't find an appropriate pool for type of {objectType.ToString()}"));

            return null;
        }
    }
}