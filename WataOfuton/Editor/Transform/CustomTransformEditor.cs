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
    [CustomEditor(typeof(Transform))]
    public partial class CustomTransformEditor : Editor
    {
        private Transform _transform;
        private Editor customTransformEditor;


        private void OnEnable()
        {
            _transform = (Transform)target;
            customTransformEditor = CreateEditor(_transform, Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector", true));

            // _transform.localPosition = RoundVec3(_transform.localPosition);
            // _transform.localEulerAngles = RoundVec3(_transform.localEulerAngles);
            // _transform.localScale = RoundVec3(_transform.localScale);

            OnEnable_Symmetry();
            OnEnable_Ignore();

            EditorApplication.update += Update;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;

            if (customTransformEditor != null)
            {
                DestroyImmediate(customTransformEditor);
            }
        }

        private void Update()
        {
            Update_Symmetry();
            Update_Ignore();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            customTransformEditor.OnInspectorGUI();

            symmetryBoneEditorGUICore();

            ignoreChildEditorGUICore();
        }

        private Vector3 RoundVec3(Vector3 target)
        {
            float factor = 1e5f; // Used to shift the decimal place

            target.x = Mathf.Round(target.x * factor) / factor;
            target.y = Mathf.Round(target.y * factor) / factor;
            target.z = Mathf.Round(target.z * factor) / factor;

            return target;
        }

    }
}