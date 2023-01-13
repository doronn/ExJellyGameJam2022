using EndlessJumper.Scripts.Tower.Interfaces;
using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Concrete
{
    public class BasePoolableObject : IPoolableObject
    {
        public Transform ObjectInstance { get; private set; }
        public virtual void OnAdded(Transform objectInstance)
        {
            ObjectInstance = objectInstance;
        }

        public virtual void OnRemoved()
        {
            ObjectInstance = null;
        }
    }
}