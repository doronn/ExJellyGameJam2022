using GameJamKit.Scripts.InputlockService.Lockables;

namespace GameJamKit.Scripts.InputlockService
{
    public interface IInputLockServiceSubscription
    {
        void SubscribeLockable(BaseInputLockable inputLockable);
        void UnsubscribeLockable(BaseInputLockable inputLockable);
    }
}