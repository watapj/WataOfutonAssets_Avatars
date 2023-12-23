/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Please save this script in a folder named "Editor".
 * NOTE : By adding this script, an extension "Symmetry Bone Editor" will be displayed in the Transform component of all GameObjects.
 */

using UnityEngine;
using UnityEditor;

namespace WataOfuton.Tool
{
    public class MaterialPropertyAutoSetterWindow : EditorWindow
    {
        private const string MENU_ITEM_PATH = "Window/WataOfuton/MaterialPropertyAutoSetter";
        public const string PREF_KEY_ENABLE = "MaterialPropertyAutoSetterWindow.EnableAutoSet";

        public static bool isEnable;
        public static readonly string[,] targetParamNames = new string[5, 2]{{"_LightMinLimit",     "明るさの下限"},
                                                                        {"_LightMaxLimit",      "明るさの上限"},
                                                                        {"_AsUnlit",            "Unlit化"},
                                                                        {"_MonochromeLighting", "ライトのモノクロ化"},
                                                                        {"_ShadowEnvStrength",  "影色への環境光影響度"},
                                                                        };
        private static float[] forcedValues = new float[5];
        private static readonly float[] defaultValues = new float[5] { 0.05f, 1f, 0f, 0f, 0f };

        [MenuItem(MENU_ITEM_PATH)]
        public static void ShowWindow()
        {
            GetWindow<MaterialPropertyAutoSetterWindow>("Material Property Auto Setter");
        }

        private void OnEnable()
        {
            LoadForcedValues();
        }

        private void OnGUI()
        {
            GUILayout.Label("MaterialPropertyAutoSetter Settings", EditorStyles.boldLabel);
            string text = "lilToon を使用しているマテリアルのパラメータを一括して設定するエディタ拡張です。\n"
                        + "各種設定をプルダウンにて選択した後、最下部の [Save Settings] を押してください。\n"
                        + "マテリアルを選択した際に自動で設定が反映されます。";
            EditorGUILayout.HelpBox(text, MessageType.Info);

            EditorGUILayout.Space(5);
            isEnable = EditorGUILayout.Toggle("Enable Auto Setter", isEnable);
            // GUILayout.Label("Forced Values", EditorStyles.boldLabel);
            for (int i = 0; i < forcedValues.Length; i++)
            {
                forcedValues[i] = EditorGUILayout.FloatField(targetParamNames[i, 1], forcedValues[i]);
            }

            if (GUILayout.Button("Save"))
            {
                SaveForcedValues();
            }
        }

        private void LoadForcedValues()
        {
            isEnable = EditorPrefs.GetBool(PREF_KEY_ENABLE, false);
            for (int i = 0; i < forcedValues.Length; i++)
            {
                forcedValues[i] = EditorPrefs.GetFloat(targetParamNames[i, 0], defaultValues[i]);
            }
        }

        private void SaveForcedValues()
        {
            EditorPrefs.SetBool(PREF_KEY_ENABLE, isEnable);
            for (int i = 0; i < forcedValues.Length; i++)
            {
                EditorPrefs.SetFloat(targetParamNames[i, 0], forcedValues[i]);
            }
            Debug.Log("Save Setting.");
        }
    }

    [InitializeOnLoad]
    public class MaterialPropertyAutoSetter
    {
        // 変更対象のシェーダ名
        private static readonly string targetShaderName = "lilToon";

        static MaterialPropertyAutoSetter()
        {
            Enable();
        }

        public static void Enable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        public static void Disable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        static void OnSelectionChanged()
        {
            if (!EditorPrefs.GetBool(MaterialPropertyAutoSetterWindow.PREF_KEY_ENABLE)) return;

            string[,] targetParamNames = MaterialPropertyAutoSetterWindow.targetParamNames;
            // forcedValues をロード
            float[] forcedValues = new float[5];
            for (int i = 0; i < forcedValues.Length; i++)
            {
                forcedValues[i] = EditorPrefs.GetFloat(targetParamNames[i, 0]);
            }

            foreach (var obj in Selection.objects)
            {
                var mat = obj as Material;
                if (mat != null && mat.shader.name.Contains(targetShaderName))
                {
                    for (int i = 0; i < targetParamNames.Length / 2; i++)
                    {
                        if (mat.HasProperty(targetParamNames[i, 0]))
                        {
                            if (mat.GetFloat(targetParamNames[i, 0]) != forcedValues[i])
                            {
                                mat.SetFloat(targetParamNames[i, 0], forcedValues[i]);
                                Debug.Log("Set " + targetParamNames[i, 1] + " of " + mat.name + " to " + forcedValues[i]);
                            }
                        }

                    }
                }
            }
        }
    }
}