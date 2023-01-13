namespace EndlessJumper.Scripts.Tower.Interfaces
{
    public interface IObjectPoolFactory
    {
        ObjectPoolManager Create(PoolObjectType objectType);
    }
}