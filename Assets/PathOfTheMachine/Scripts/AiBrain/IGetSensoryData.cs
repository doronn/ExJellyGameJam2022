namespace AiBrain
{
    public interface IGetSensoryData
    {
        float[] Current { get; }
        float Score { get; }
        bool Alive { get; }
    }
}