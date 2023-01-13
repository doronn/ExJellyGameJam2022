namespace EndlessJumper.Scripts.Tower.Interfaces
{
    public interface IFloorData : IPoolableObject
    {
        float Width { get; }
        float X { get; }
        float Y { get; }
        void Init(float yPosition);

    }
}