/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Please save this script in a folder named "Editor".
 * NOTE : By adding this script, an extension "Symmetry Bone Editor" will be displayed in the Transform component of all GameObjects.
 */

using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace WataOfuton.Tool
{
    public partial class CustomTransformEditor
    {
        private bool _ignoreEnabled = false;
        private bool _appryScale = false;

        private Vector3[] _childLastPosition, _childLastRotation, _childLastScale;
        private bool[] _childIgnoreToggle;


        private void OnEnable_Ignore()
        {
            _childLastPosition = new Vector3[_transform.childCount];
            _childLastRotation = new Vector3[_transform.childCount];
            _childLastScale = new Vector3[_transform.childCount];

            _childIgnoreToggle = new bool[_transform.childCount];
            // SaveLastTransform();
        }

        private void Update_Ignore()
        {
            if (_ignoreEnabled)
            {
                if (_transform.localPosition != _lastPosition ||
                    _transform.localEulerAngles != _lastRotation ||
                    _transform.localScale != _lastScale)
                {
                    IgnoreChildTransform();
                    // SaveLastTransform();
                }
            }

        }

        private void SaveLastTransform()
        {
            int c = 0;
            foreach (Transform child in _transform)
            {
                _childLastPosition[c] = child.position;
                _childLastRotation[c] = child.eulerAngles;
                _childLastScale[c] = child.localScale;
                c++;
            }
        }

        private void ignoreChildEditorGUICore()
        {
            EditorGUILayout.Space(10);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ignore Child Transform Editor", EditorStyles.boldLabel, GUILayout.Width(150));
            EditorGUIUtility.labelWidth = 15;
            _ignoreEnabled = EditorGUILayout.Toggle(_ignoreEnabled);
            EditorGUIUtility.labelWidth = 0;  // Reset to default
            EditorGUILayout.EndHorizontal();

            Undo.RecordObject(_transform, "Ignore Child Transform Change");

            if (_ignoreEnabled)
            {
                if (_transform.childCount > 0)
                {
                    // EditorGUILayout.HelpBox("Ignore Child is enabled.(It does not apply to the Scale.)", MessageType.Info);
                    EditorGUILayout.HelpBox("Ignore Child is enabled.", MessageType.Info);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Appry Scale", GUILayout.Width(150));
                    EditorGUIUtility.labelWidth = 15;
                    _appryScale = EditorGUILayout.Toggle(_appryScale);
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();

                    Undo.RegisterCompleteObjectUndo(_transform, "Transform Change Ignore Child");

                    int c = 0;
                    using (new EditorGUILayout.VerticalScope("Box"))
                    {
                        // Box のスタイルが適応される。
                        foreach (Transform child in _transform)
                        {
                            // _childIgnoreToggle[c] = EditorGUILayout.Toggle("Ignore " + child.name, _childIgnoreToggle[c]);
                            // EditorGUILayout.ObjectField("Ignore Child " + c.ToString(), child, typeof(GameObject), true);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Ignore Childs " + c.ToString(), GUILayout.Width(170));
                            EditorGUIUtility.labelWidth = 5;
                            _childIgnoreToggle[c] = EditorGUILayout.Toggle(_childIgnoreToggle[c]);
                            EditorGUIUtility.labelWidth = 200;
                            EditorGUILayout.ObjectField(child, typeof(GameObject), true);
                            c++;
                            EditorGUIUtility.labelWidth = 0;  // Reset to default

                            EditorGUILayout.EndHorizontal();
                            Undo.RegisterCompleteObjectUndo(child, "Transform Change Ignore Child");

                        }
                    }

                    if (_symmetryEnabled)
                    {
                        string symmetricalBoneName = GetSymmetricalBoneName(_transform.name);
                        if (symmetricalBoneName != null)
                        {
                            symmetricalBone = FindTransformRecursively(_transform.parent, symmetricalBoneName);

                            EditorGUILayout.Space(10);
                            c = 0;
                            foreach (Transform child in symmetricalBone)
                            {
                                EditorGUILayout.ObjectField("Ignore Child in SymmetricalBone " + (c++).ToString(), child, typeof(GameObject), true);
                                Undo.RegisterCompleteObjectUndo(child, "Transform Change Ignore Child");
                            }
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("This object does not have a child.", MessageType.Warning);
                    return;
                }

                EditorGUILayout.Space(10);
            }
            else
            {
                SaveLastTransform();
            }

            if (EditorGUI.EndChangeCheck() && _ignoreEnabled)
            {
                IgnoreChildTransform();
            }
        }

        private void IgnoreChildTransform()
        {
            if (_transform == null || _transform.childCount == 0) return;

            Matrix4x4 mat = _transform.worldToLocalMatrix;
            Vector3 right = mat.GetColumn(0);
            Vector3 up = mat.GetColumn(1);
            Vector3 forward = mat.GetColumn(2);

            float x = right.magnitude;
            float y = up.magnitude;
            float z = forward.magnitude;

            int c = 0;
            foreach (Transform child in _transform)
            {
                if (_childIgnoreToggle[c])
                {
                    if (_appryScale)
                    {
                        child.localScale = DivideVec3(_childLastScale[c], _transform.localScale);
                    }
                    child.position = _childLastPosition[c];
                    child.eulerAngles = _childLastRotation[c];
                }
                c++;
            }
        }

        private static Vector3 DivideVec3(Vector3 v1, Vector3 v2)
        {
            float x = v2.x != 0f ? v1.x / v2.x : 0f;
            float y = v2.y != 0f ? v1.y / v2.y : 0f;
            float z = v2.z != 0f ? v1.z / v2.z : 0f;
            return new Vector3(x, y, z);
        }
    }
}