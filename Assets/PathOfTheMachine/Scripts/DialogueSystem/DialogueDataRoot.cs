using UnityEngine;

namespace PathOfTheMachine.Scripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueDataRoot", menuName = "Dialogue/Create Root", order = 0)]
    public class DialogueDataRoot : ScriptableObject
    {
        [SerializeField]
        private DialogueNode _dialogueNode;
    }
}