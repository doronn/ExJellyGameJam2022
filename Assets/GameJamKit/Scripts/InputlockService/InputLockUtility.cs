using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameJamKit.Scripts.InputlockService
{
   public class InputLockUtility : MonoBehaviour
   {
      private IInputLockService _inputLockService;
      private List<InputLock> _allInputInputLock = new();

      [Inject]
      public void Inject(IInputLockService inputLockService)
      {
          _inputLockService = inputLockService;
      }
      
      public void LockAllInput()
      {
         _allInputInputLock?.Add(_inputLockService.LockAllInputs());
      }

      public void LockGuiRaycaster()
      {
         LockSomeInput( new[] { InputLockTag.GuiRaycaster });
      }
      
      public void LockPhysicsRaycaster()
      {
         LockSomeInput( new[] { InputLockTag.PhysicsRaycaster });
      }
      
      public void LockCube()
      {
         LockSomeInput( new[] { InputLockTag.Cube });
      }
      
      public void LockSphere()
      {
         LockSomeInput( new[] { InputLockTag.Sphere });
      }

      public void UnlockAllInput()
      {
         foreach (var inputLock in _allInputInputLock)
         {
            _inputLockService.UnlockInput(inputLock);
         }
      }

      private void LockSomeInput(InputLockTag[] tags)
      {
         _allInputInputLock?.Add(_inputLockService.LockInput(tags));
      }

      private InputLock _inputLock;
   }
}
