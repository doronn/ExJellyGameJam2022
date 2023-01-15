namespace Sensory
{
    public interface ISensor
    {
        int OwnerId { get; }
        void RegisterListener(ISensorListener sensorListener, int ownerId);
    }
}