using UnityEngine;

namespace EndlessJumper.Scripts.Tower.SettingsData
{
    [CreateAssetMenu(fileName = "LevelSegmentsSettings", menuName = "Tower/Level SegmentsSettings", order = 0)]
    public class LevelsSettings : ScriptableObject
    {
        [field: SerializeField] public int FloorsInSegment { get; private set; }

        [field: SerializeField] public int LevelSegmentsCount { get; private set; }
        [field: SerializeField] public float LevelSegmentsHeight { get; private set; }
        [field: SerializeField] public float VerticalMovementSpeed { get; private set; }
    }
}