/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Please save this script in a folder named "Editor".
 * NOTE : By adding this script, an extension "Symmetry Bone Editor" will be displayed in the Transform component of all GameObjects.
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace WataOfuton.Tool
{
    public class BoneCopierWindow : EditorWindow
    {
        private const string MENU_ITEM_PATH = "Window/WataOfuton/Bone Transform Copier";
        private GameObject sourceArmature;
        private GameObject targetArmature;
        private string sourcePrefix = "";
        private string sourceSuffix = "";
        private string targetPrefix = "";
        private string targetSuffix = "";
        private Vector2 scrollPos;

        private class BoneInfo
        {
            public bool ShouldCopy = true;
            public Transform TargetTransform;
        }

        private Dictionary<string, BoneInfo> boneCopyFlags = new Dictionary<string, BoneInfo>();

        [MenuItem(MENU_ITEM_PATH)]
        public static void ShowWindow()
        {
            GetWindow<BoneCopierWindow>("Bone Transform Copier");
        }

        [MenuItem("GameObject/WataOfuton/Bone Transform Copier", false, 21)]
        public static void ShowWindowFromGameObject()
        {
            var window = GetWindow<BoneCopierWindow>("Bone Transform Copier");
            window.targetArmature = Selection.activeGameObject;
        }

        private void OnGUI()
        {
            GUILayout.Label("Set source and target armatures", EditorStyles.boldLabel);

            sourceArmature = (GameObject)EditorGUILayout.ObjectField("Source Armature", sourceArmature, typeof(GameObject), true);
            targetArmature = (GameObject)EditorGUILayout.ObjectField("Target Armature", targetArmature, typeof(GameObject), true);

            GUILayout.Label("Prefix and suffix", EditorStyles.boldLabel);
            sourcePrefix = EditorGUILayout.TextField("Source Prefix", sourcePrefix);
            sourceSuffix = EditorGUILayout.TextField("Source Suffix", sourceSuffix);
            targetPrefix = EditorGUILayout.TextField("Target Prefix", targetPrefix);
            targetSuffix = EditorGUILayout.TextField("Target Suffix", targetSuffix);

            if (GUILayout.Button("Update Bone List"))
            {
                UpdateBoneList();
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var pair in boneCopyFlags)
            {
                var boneInfo = pair.Value;
                EditorGUILayout.BeginHorizontal();
                boneInfo.ShouldCopy = EditorGUILayout.Toggle(boneInfo.ShouldCopy, GUILayout.Width(15));
                EditorGUIUtility.labelWidth = 150;
                boneInfo.TargetTransform = (Transform)EditorGUILayout.ObjectField(boneInfo.TargetTransform, typeof(Transform), true);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Apply"))
            {
                RecordUndo();
                CopyTransforms(sourceArmature, targetArmature);
            }
        }

        private void RecordUndo()
        {
            foreach (var pair in boneCopyFlags)
            {
                var boneInfo = pair.Value;
                Undo.RegisterCompleteObjectUndo(boneInfo.TargetTransform, "Transform Change Ignore Child");
            }
        }

        private void UpdateBoneList()
        {
            boneCopyFlags.Clear();

            if (sourceArmature == null || targetArmature == null)
            {
                Debug.LogError("Source or target is not set");
                return;
            }

            ProcessTransform(sourceArmature.transform, targetArmature.transform);
        }

        private void ProcessTransform(Transform sourceTransform, Transform targetTransform)
        {
            string sourceName = sourceTransform.name;
            if (!string.IsNullOrEmpty(sourcePrefix)) sourceName = sourceName.Replace(sourcePrefix, "");

            if (!string.IsNullOrEmpty(sourceSuffix)) sourceName = sourceName.Replace(sourceSuffix, "");

            string targetChildName = targetPrefix + sourceName + targetSuffix;
            Transform targetChild = targetTransform.FindDeepChild(targetChildName, StringComparison.OrdinalIgnoreCase);
            if (targetChild != null)
            {
                boneCopyFlags.Add(targetChildName, new BoneInfo { TargetTransform = targetChild });
            }

            for (int i = 0; i < sourceTransform.childCount; i++)
            {
                ProcessTransform(sourceTransform.GetChild(i), targetTransform);
            }
        }

        private void CopyTransforms(GameObject source, GameObject target)
        {
            if (source == null || target == null)
            {
                Debug.LogError("Source or target is not set");
                return;
            }

            CopyTransformRecursive(source.transform, target.transform);
        }

        private void CopyTransformRecursive(Transform sourceChild, Transform target)
        {
            string sourceName = sourceChild.name;
            if (!string.IsNullOrEmpty(sourcePrefix)) sourceName = sourceName.Replace(sourcePrefix, "");

            if (!string.IsNullOrEmpty(sourceSuffix)) sourceName = sourceName.Replace(sourceSuffix, "");

            string targetChildName = targetPrefix + sourceName + targetSuffix;
            var targetChild = target.FindDeepChild(targetChildName, StringComparison.OrdinalIgnoreCase);
            if (targetChild != null && boneCopyFlags.ContainsKey(targetChildName) && boneCopyFlags[targetChildName].ShouldCopy)
            {
                targetChild.localPosition = sourceChild.localPosition;
                targetChild.localRotation = sourceChild.localRotation;
                targetChild.localScale = sourceChild.localScale;
            }

            for (int i = 0; i < sourceChild.childCount; i++)
            {
                CopyTransformRecursive(sourceChild.GetChild(i), target);
            }
        }
    }

    public static class TransformDeepChildExtension
    {
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName, StringComparison comparisonType)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName, comparisonType);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}