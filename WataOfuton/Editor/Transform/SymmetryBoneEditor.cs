/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Example : https://twitter.com/wata_pj/status/1656662228572196866
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
        private Transform symmetricalBone;
        private bool _symmetryEnabled = false;
        private Vector3 _positionInvertFlags = new Vector3(-1, 1, 1);
        private Vector3 _rotationInvertFlags = new Vector3(1, -1, -1);
        private Vector3 _lastPosition, _lastRotation, _lastScale;

        private void OnEnable_Symmetry()
        {
            _lastPosition = _transform.localPosition;
            _lastRotation = _transform.localEulerAngles;
            _lastScale = _transform.localScale;
        }

        private void Update_Symmetry()
        {
            if (_symmetryEnabled)
            {
                if (_transform.localPosition != _lastPosition ||
                    _transform.localEulerAngles != _lastRotation ||
                    _transform.localScale != _lastScale)
                {
                    ApplySymmetry();
                    _lastPosition = _transform.localPosition;
                    _lastRotation = _transform.localEulerAngles;
                    _lastScale = _transform.localScale;
                }
            }
        }

        private void symmetryBoneEditorGUICore()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Symmetry Bone Editor", EditorStyles.boldLabel, GUILayout.Width(150));
            EditorGUIUtility.labelWidth = 15;
            _symmetryEnabled = EditorGUILayout.Toggle(_symmetryEnabled);
            EditorGUIUtility.labelWidth = 0;  // Reset to default
            EditorGUILayout.EndHorizontal();

            if (_symmetryEnabled)
            {
                Undo.RecordObject(_transform, "Transform Change Symmetry");

                // Display symmetricalBone in Transform component
                string symmetricalBoneName = GetSymmetricalBoneName(_transform.name);
                if (symmetricalBoneName != null)
                {
                    symmetricalBone = FindTransformRecursively(_transform.parent, symmetricalBoneName);
                    if (symmetricalBone != null)
                    {
                        EditorGUILayout.HelpBox("Symmetry is enabled. Changes to this transform will automatically affect its symmetrical counterpart.", MessageType.Info);
                        EditorGUILayout.ObjectField("Symmetrical Bone", symmetricalBone, typeof(GameObject), true);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Symmetry bone not found.", MessageType.Info);
                        return;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("This object does not have a symmetrical counterpart.", MessageType.Warning);
                    return;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Invert Position Axes", GUILayout.Width(150));
                EditorGUIUtility.labelWidth = 15;
                _positionInvertFlags.x = EditorGUILayout.Toggle("X", _positionInvertFlags.x == -1) ? -1 : 1;
                _positionInvertFlags.y = EditorGUILayout.Toggle("Y", _positionInvertFlags.y == -1) ? -1 : 1;
                _positionInvertFlags.z = EditorGUILayout.Toggle("Z", _positionInvertFlags.z == -1) ? -1 : 1;
                EditorGUIUtility.labelWidth = 0;  // Reset to default
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Invert Rotation Axes", GUILayout.Width(150));
                EditorGUIUtility.labelWidth = 15;
                _rotationInvertFlags.x = EditorGUILayout.Toggle("X", _rotationInvertFlags.x == -1) ? -1 : 1;
                _rotationInvertFlags.y = EditorGUILayout.Toggle("Y", _rotationInvertFlags.y == -1) ? -1 : 1;
                _rotationInvertFlags.z = EditorGUILayout.Toggle("Z", _rotationInvertFlags.z == -1) ? -1 : 1;
                EditorGUIUtility.labelWidth = 0;  // Reset to default
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }

            if (EditorGUI.EndChangeCheck() && _symmetryEnabled)
            {
                ApplySymmetry();
            }
        }

        private void ApplySymmetry()
        {
            if (symmetricalBone == null) return;
            if (_transform == null) return;

            symmetricalBone.localPosition = Vector3.Scale(_transform.localPosition, _positionInvertFlags);
            symmetricalBone.localEulerAngles = Vector3.Scale(_transform.localEulerAngles, _rotationInvertFlags);
            symmetricalBone.localScale = _transform.localScale;
        }

        private string GetSymmetricalBoneName(string boneName)
        {
            // 数値接尾辞を抽出するための正規表現
            var regex = new System.Text.RegularExpressions.Regex(@"(\.\d+)$");
            var match = regex.Match(boneName);
            string numberSuffix = match.Success ? match.Groups[1].Value : "";

            // 数値接尾辞を除いたベース名を取得
            string baseName = regex.Replace(boneName, "");

            // 対称性を確認するパターン
            string[] patterns = { "_L", "_R", ".L", ".R", " L", " R", "_l", "_r", ".l", ".r", " l", " r" };
            foreach (string pattern in patterns)
            {
                if (baseName.EndsWith(pattern, System.StringComparison.OrdinalIgnoreCase))
                {
                    string oppositePattern = pattern.ToLower().Contains("l") ? pattern.Replace("L", "R").Replace("l", "r") : pattern.Replace("R", "L").Replace("r", "l");
                    return baseName.Substring(0, baseName.Length - pattern.Length) + oppositePattern + numberSuffix;
                }
            }

            if (boneName.Contains("Left"))
            {
                return boneName.Replace("Left", "Right");
            }
            else if (boneName.Contains("Right"))
            {
                return boneName.Replace("Right", "Left");
            }
            else if (boneName.Contains("left"))
            {
                return boneName.Replace("left", "right");
            }
            else if (boneName.Contains("right"))
            {
                return boneName.Replace("right", "left");
            }

            return null;
        }

        private Transform FindTransformRecursively(Transform startTransform, string name, List<Transform> checkedTransforms = null)
        {
            if (checkedTransforms == null)
            {
                checkedTransforms = new List<Transform>();
            }

            if (checkedTransforms.Contains(startTransform))
            {
                return null;
            }

            checkedTransforms.Add(startTransform);

            // Check the current transform for the target name
            foreach (Transform child in startTransform)
            {
                if (child.name.Equals(name))
                {
                    return child;
                }
            }

            // Recursively check the children
            foreach (Transform child in startTransform)
            {
                Transform result = FindTransformRecursively(child, name, checkedTransforms);

                if (result != null)
                {
                    return result;
                }
            }

            // If the target transform was not found in the current transform's hierarchy, go one level up (if possible)
            if (startTransform.parent != null)
            {
                return FindTransformRecursively(startTransform.parent, name, checkedTransforms);
            }

            // If the target transform was not found and there are no more parents to check, return null
            return null;
        }
    }

}
