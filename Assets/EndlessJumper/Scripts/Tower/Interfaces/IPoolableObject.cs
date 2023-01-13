using UnityEngine;

namespace EndlessJumper.Scripts.Tower.Interfaces
{
    public interface IPoolableObject
    {
        Transform ObjectInstance { get; }

        void OnAdded(Transform objectInstance);
        void OnRemoved(); 
    }
}