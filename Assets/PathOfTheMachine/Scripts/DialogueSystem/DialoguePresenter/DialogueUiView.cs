using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PathOfTheMachine.Scripts.DialogueSystem.DialoguePresenter
{
    public class DialogueUiView : MonoBehaviour
    {
        public UIDocument UIDocument;

        private void Start()
        {
            UIDocument.rootVisualElement.Q("button");
        }
    }
}