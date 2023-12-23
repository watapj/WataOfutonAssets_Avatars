using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WataOfuton.Tool.MMDSetup.Editor
{
    [CustomEditor(typeof(MMDSetup))]
    public class MMDSetupEditor : UnityEditor.Editor
    {
        // SerializedProperty animator;
        SerializedProperty faceMesh;
        MMDSetup MMDSetup;

        void OnEnable()
        {
            // animator = serializedObject.FindProperty(nameof(MMDSetup.animator));
            faceMesh = serializedObject.FindProperty(nameof(MMDSetup.faceMesh));
            MMDSetup = target as MMDSetup;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // EditorGUILayout.PropertyField(animator);
            EditorGUILayout.PropertyField(faceMesh);
            if (faceMesh.objectReferenceValue == null)
            {
                var faceT = MMDSetup.transform.Find("Body");
                if (faceT == null)
                {
                    faceT = MMDSetup.transform.Find("Face");
                }
                if (faceT != null)
                {
                    faceMesh.objectReferenceValue = faceT;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {

        }


        // public static string[] blendShapeMappings = new string[]
        // {
        //     "あ", "い", "う", "え", "お", "ん", "∧", "ω", "ω□", "ワ", "▲",

        //     "怒り", "困る", "頬染め",

        //     "星目",
        //     "白目",
        //     "びっくり", // 14
        //     "じと目",
        //     "はぅ",
        //     "なごみ",
        //     "笑い",
        //     "まばたき",

        //     "ウィンク", // 20
        //     "ウィンク２",
        //     "ウィンク右", // 22
        //     "ウィンク２右",
        //     "ｳｨﾝｸ2右",
        // };


        // public static string[,] blendShapeMappingsTranslate = new string[,]
        // {
        //     // https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/i/0b7b5e4b-c62e-41f7-8ced-1f3e58c4f5bf/d5nbmvp-5779f5ac-d476-426c-8ee6-2111eff8e76c.png
        //     {"あ", "a"},
        //     {"い", "i"},
        //     {"う", "u"},
        //     {"え", "e"},
        //     {"お", "o"},
        //     {"ん", "n"},
        //     {"▲", "Mouse_2"},
        //     {"∧", "Mouse_2"},
        //     {"ω", "Omega"},

        //     {"怒り", "Get angry"},
        //     {"困る", "Trouble"},
        //     // {"頬染め", "a"},

        //     {"星目", "EyeStar"},
        //     // {"白目", "a"},
        //     {"びっくり", "Ha!!!"},
        //     {"じと目", "Jito-eye"},
        //     {"はぅ", "> <"},
        //     {"なごみ", "Howawa"},
        //     {"笑い", "Smile"},
        //     // {"まばたき", "Blink"},

        //     {"ウィンク", "Wink"},
        //     {"ウィンク２", "Wink-b"},
        //     {"ウィンク右", "Wink-a"},
        //     {"ウィンク２右", "Wink-c"},
        // };
    }
}
