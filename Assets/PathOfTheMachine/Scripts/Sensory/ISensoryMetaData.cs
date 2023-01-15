namespace Sensory
{
    public interface ISensoryMetaData
    {
        float[] OnRequestSensoryData(float[] arrayBuffer);
        int GetForwardSensorIndex();
        bool GetAnySensorHit();
    }
}