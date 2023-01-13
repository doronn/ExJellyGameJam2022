using System;
using UnityEngine;

namespace PathOfTheMachine.Scripts.DialogueSystem
{
    [Serializable]
    public class DialogueStringTree
    {
        public DialogueStringTree(string dialogueOption, DialogueNode dialogueNode)
        {
            _dialogueOption = dialogueOption;
            _dialogueNode = dialogueNode;
        }

        [SerializeField, TextArea]
        private string _dialogueOption;
        
        [SerializeField]
        private DialogueNode _dialogueNode;

    }
}