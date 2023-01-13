using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameJamKit.Scripts.Utils.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameJamKit.Scripts.Utils.Editor.CustomInspectors
{
    public class BehaviourButtonsHelper
    {
        private static readonly object[] EmptyParamList = Array.Empty<object>();
        private static readonly object[] StringParamsList = new object[1];

        private IList<MethodInfo> _methods = new List<MethodInfo>();
        private IList<(MethodInfo method, string[] parameters)> _methodsWithStringParam = new List<(MethodInfo method, string[] parameters)>();
        private Object _targetObject;

        public void Init(Object targetObject)
        {
            this._targetObject = targetObject;
            _methods =
                targetObject.GetType()
                            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(methodInfo =>
                                methodInfo.GetCustomAttributes(typeof(ButtonAttribute), false).Length == 1 &&
                                methodInfo.GetParameters().Length == 0 &&
                                !methodInfo.ContainsGenericParameters
                            ).ToList();
            
            _methodsWithStringParam =
                targetObject.GetType()
                            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Instance).Where(methodInfo =>
                                methodInfo.GetCustomAttributes(typeof(ButtonAttribute), false)
                                          .Length == 1 &&
                                methodInfo.GetParameters().Length == 1 &&
                                methodInfo.GetParameters()[0].ParameterType == typeof(string)
                            ).Select(info => (info, new string[1])).ToList();
        }

        public void DrawButtons()
        {
            if (_methods.Count > 0 || _methodsWithStringParam.Count > 0)
            {
                EditorGUILayout.HelpBox("Click to execute methods!", MessageType.None);
                ShowMethodButtons();
            }
        }

        private void ShowMethodButtons()
        {
            foreach (var method in _methods)
            {
                var buttonText = ObjectNames.NicifyVariableName(method.Name);
                if (GUILayout.Button(buttonText)) method.Invoke(_targetObject, EmptyParamList);
            }

            for (var index = 0; index < _methodsWithStringParam.Count; index++)
            {
                var methodWithStringParam = _methodsWithStringParam[index];
                var withStringParam = methodWithStringParam;
                var buttonText = ObjectNames.NicifyVariableName(withStringParam.method.Name);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear params"))
                {
                    withStringParam.parameters[0] =
                        GUILayout.TextArea(string.Empty);
                }
                else
                {
                    withStringParam.parameters[0] = GUILayout.TextArea(withStringParam.parameters[0]);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button(buttonText))
                {
                    StringParamsList[0] = withStringParam.parameters[0];
                    withStringParam.method.Invoke(_targetObject, StringParamsList);
                }
            }
        }
    }


    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class BehaviourButtonsEditor : UnityEditor.Editor
    {
        private readonly BehaviourButtonsHelper _behaviourButtonsHelper = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _behaviourButtonsHelper.DrawButtons();
        }

        private void OnEnable()
        {
            _behaviourButtonsHelper.Init(target);
        }
    }
}