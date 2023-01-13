using System;
using System.Collections.Generic;
using System.Linq;
using GameJamKit.Scripts.Utils.Attributes;
using UnityEngine;

namespace PathOfTheMachine.Scripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Create Node", order = 1)]
    [Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField, TextArea]
        private string _dialogueNodeText;

        [SerializeField]
        private List<DialogueStringTree> _dialogueTree;

#if UNITY_EDITOR
        [Button]
        public void AddDialogResponse(string text)
        {
            Debug.Log($"Adding text with new node:\n{text}");
            var dialogueNode = CreateInstance<DialogueNode>();
            dialogueNode.name = $"Sub_dialog_{_dialogueTree.Count + 1}";
            UnityEditor.AssetDatabase.AddObjectToAsset( dialogueNode , this );
            _dialogueTree.Add(new DialogueStringTree(text, dialogueNode));
            
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}