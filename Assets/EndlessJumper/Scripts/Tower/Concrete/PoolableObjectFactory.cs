using EndlessJumper.Scripts.Tower.Concrete;

namespace EndlessJumper.Scripts.Tower.Interfaces
{
    public class PoolableObjectFactory : IPoolableObjectFactory
    {
        public IPoolableObject Create()
        {
            return new BasePoolableObject();
        }
    }
}