using UnityEngine;

namespace PathOfTheMachine.Scripts.GameWorld
{
    [CreateAssetMenu(fileName = "WorldConfiguration", menuName = "World/Create World Configuration", order = 0)]
    public class WorldConfiguration : ScriptableObject
    {
        [field: SerializeField]
        public Vector2Int WorldSize { get; private set; }
        
        [field: SerializeField]
        public Vector2 LevelSize { get; private set; }
        
        [field: SerializeField]
        public GameObject LevelPrefab { get; private set; }
        
        [field: SerializeField]
        public GameObject PlayerPrefab { get; private set; }
        
        [field: SerializeField]
        public GameObject BotPrefab { get; private set; }
        
        [field: SerializeField]
        public Vector2[] StartLocations { get; private set; }
    }
}