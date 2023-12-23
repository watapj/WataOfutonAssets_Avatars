/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Please save this script in a folder named "Editor".
 * NOTE : By adding this script, an extension "Symmetry Bone Editor" will be displayed in the Transform component of all GameObjects.
 */

using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace WataOfuton.Tool
{
    public class CustomAssetImportWindow : EditorWindow
    {
        private const string MENU_ITEM_PATH = "Window/WataOfuton/CustomAssetImporter";
        public const string PREF_KEY_ENABLE_MODEL = "CustomAssetImportWindow.EnableCustomAssetImportProcessor.Model";
        public const string PREF_KEY_ENABLE_TEX = "CustomAssetImportWindow.EnableCustomAssetImportProcessor.Texture";
        private static string[] skipOption = new string[] { "( Skip )" };
        private static string[] meshCompressionOptions = new string[] { "Off", "Low", "Medium", "High" };
        private static string[] modelImporterNormalsOptions = new string[] { "Import", "Calculate", "None" };
        private static string[] blendShapeNormalsOptions = new string[] { "Import", "Calculate", "None" };
        private static string[] compressionQualityOptions = new string[] { "10", "50", "100" };
        private static bool bakeAxisConversion;
        private static bool importBlendShapes;
        private static bool importVisibility;
        private static bool importCameras;
        private static bool importLights;
        private static bool preserveHierarchy;
        private static bool sortHierarchyByName;
        private static bool isReadable;
        private static bool weldVertices;
        private static bool legacyBlendShapeNormals;

        // public static string[] maxTextureSizeOptions = new string[] { "1024", "2048", "4096", "8192" };
        private static bool streamingMipmaps;
        private static bool crunchedCompression;

        private static int selectedMeshCompressionIndex;
        private static int selectedBlendShapeNormalsIndex;
        private static int selectedModelImporterNormalsIndex;
        private static int selectedCompressionQualityIndex;
        // private static int selectedMaxTextureSizeIndex;
        public static bool isEnableModelImport;
        public static bool isEnableTextureImport;


        [MenuItem(MENU_ITEM_PATH)]
        public static void ShowWindow()
        {
            GetWindow<CustomAssetImportWindow>("Custom Asset Import");
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        private void OnGUI()
        {
            GUILayout.Label("Custom Asset Import Settings", EditorStyles.boldLabel);
            string text = "3Dモデルやテクスチャをインポートする際の Import Settings を一括して設定するエディタ拡張です。\n"
                        + "各種設定をプルダウンにて選択した後、最下部の [Save Settings] を押してください。\n"
                        + "個別で設定したいパラメータは、 (skip) を選択してください。そのパラメータの設定を行いません。";
            EditorGUILayout.HelpBox(text, MessageType.Info);

            EditorGUILayout.Space(5);
            isEnableModelImport = EditorGUILayout.Toggle("[Model] Enable Auto Setter", isEnableModelImport);
            EditorGUILayout.Space(3);

            GUILayout.Label("Scene");
            bakeAxisConversion = EditorGUILayout.Toggle("Bake Axis Conversion", bakeAxisConversion);
            importBlendShapes = EditorGUILayout.Toggle("Import Blendshapes", importBlendShapes);
            importVisibility = EditorGUILayout.Toggle("Import Visibility", importVisibility);
            importCameras = EditorGUILayout.Toggle("Import Cameras", importCameras);
            importLights = EditorGUILayout.Toggle("Import Lights", importLights);
            preserveHierarchy = EditorGUILayout.Toggle("Preserve Hierarchy", preserveHierarchy);
            sortHierarchyByName = EditorGUILayout.Toggle("Sort Hierarchy By Name", sortHierarchyByName);

            EditorGUILayout.Space(3);
            GUILayout.Label("Meshes");
            isReadable = EditorGUILayout.Toggle("Read / Write", isReadable);
            selectedMeshCompressionIndex = EditorGUILayout.Popup("Mesh Compression", selectedMeshCompressionIndex, skipOption.Concat(meshCompressionOptions).ToArray());
            EditorGUILayout.Space(3);
            GUILayout.Label("Geometry");
            weldVertices = EditorGUILayout.Toggle("Weld Vertices", weldVertices);
            legacyBlendShapeNormals = EditorGUILayout.Toggle("Legacy Blend Shape Normals", legacyBlendShapeNormals);
            selectedModelImporterNormalsIndex = EditorGUILayout.Popup("Model Importer Normals", selectedModelImporterNormalsIndex, skipOption.Concat(modelImporterNormalsOptions).ToArray());
            selectedBlendShapeNormalsIndex = EditorGUILayout.Popup("Blend Shape Normals", selectedBlendShapeNormalsIndex, skipOption.Concat(blendShapeNormalsOptions).ToArray());

            EditorGUILayout.Space(10);
            isEnableTextureImport = EditorGUILayout.Toggle("[Texture] Enable Auto Setter", isEnableTextureImport);
            EditorGUILayout.Space(5);
            // selectedMaxTextureSizeIndex = EditorGUILayout.Popup("Texture Max Size", selectedMaxTextureSizeIndex, skipOption.Concat(maxTextureSizeOptions).ToArray());
            streamingMipmaps = EditorGUILayout.Toggle("Streaming Mipmaps", streamingMipmaps);
            crunchedCompression = EditorGUILayout.Toggle("Crunched Compression", crunchedCompression);
            if (crunchedCompression)
            {
                selectedCompressionQualityIndex = EditorGUILayout.Popup("Compression Quality", selectedCompressionQualityIndex, skipOption.Concat(compressionQualityOptions).ToArray());
            }

            EditorGUILayout.Space(5);
            if (GUILayout.Button("Save Settings"))
            {
                SaveSettings();
            }
        }

        private void LoadSettings()
        {
            isEnableModelImport = EditorPrefs.GetBool(PREF_KEY_ENABLE_MODEL, false);
            bakeAxisConversion = EditorPrefs.GetBool("CustomAssetImport.BakeAxisConversion", false);
            importBlendShapes = EditorPrefs.GetBool("CustomAssetImport.ImportBlendshapes", true);
            importVisibility = EditorPrefs.GetBool("CustomAssetImport.ImportVisibility", false);
            importCameras = EditorPrefs.GetBool("CustomAssetImport.ImportCameras", false);
            importLights = EditorPrefs.GetBool("CustomAssetImport.ImportLights", false);
            preserveHierarchy = EditorPrefs.GetBool("CustomAssetImport.PreserveHierarchy", false);
            sortHierarchyByName = EditorPrefs.GetBool("CustomAssetImport.SortHierarchyByName", false);
            isReadable = EditorPrefs.GetBool("CustomAssetImport.IsReadable", true);
            weldVertices = EditorPrefs.GetBool("CustomAssetImport.WeldVertices", true);
            legacyBlendShapeNormals = EditorPrefs.GetBool("CustomAssetImport.LegacyBlendShapeNormals", false);
            selectedModelImporterNormalsIndex = EditorPrefs.GetInt("CustomAssetImport.ModelImporterNormals");
            selectedBlendShapeNormalsIndex = EditorPrefs.GetInt("CustomAssetImport.BlendShapeNormals");
            selectedMeshCompressionIndex = EditorPrefs.GetInt("CustomAssetImport.MeshCompression");

            isEnableTextureImport = EditorPrefs.GetBool(PREF_KEY_ENABLE_TEX, false);
            // selectedMaxTextureSizeIndex = EditorPrefs.GetInt("CustomAssetImport.MaxTextureSize");
            streamingMipmaps = EditorPrefs.GetBool("CustomAssetImport.StreamingMipmaps", true);
            crunchedCompression = EditorPrefs.GetBool("CustomAssetImport.CrunchedCompression", false);
            selectedCompressionQualityIndex = EditorPrefs.GetInt("CustomAssetImport.CompressionQuality");
        }

        private void SaveSettings()
        {
            EditorPrefs.SetBool(PREF_KEY_ENABLE_MODEL, isEnableModelImport);
            EditorPrefs.SetBool("CustomAssetImport.BakeAxisConversion", bakeAxisConversion);
            EditorPrefs.SetBool("CustomAssetImport.ImportBlendshapes", importBlendShapes);
            EditorPrefs.SetBool("CustomAssetImport.ImportVisibility", importVisibility);
            EditorPrefs.SetBool("CustomAssetImport.ImportCameras", importCameras);
            EditorPrefs.SetBool("CustomAssetImport.ImportLights", importLights);
            EditorPrefs.SetBool("CustomAssetImport.PreserveHierarchy", preserveHierarchy);
            EditorPrefs.SetBool("CustomAssetImport.SortHierarchyByName", sortHierarchyByName);
            EditorPrefs.SetBool("CustomAssetImport.IsReadable", isReadable);
            EditorPrefs.SetBool("CustomAssetImport.WeldVertices", weldVertices);
            EditorPrefs.SetBool("CustomAssetImport.LegacyBlendShapeNormals", legacyBlendShapeNormals);
            EditorPrefs.SetInt("CustomAssetImport.ModelImporterNormals", selectedModelImporterNormalsIndex);
            EditorPrefs.SetInt("CustomAssetImport.BlendShapeNormals", selectedBlendShapeNormalsIndex);
            EditorPrefs.SetInt("CustomAssetImport.MeshCompression", selectedMeshCompressionIndex);

            EditorPrefs.SetBool(PREF_KEY_ENABLE_TEX, isEnableTextureImport);
            // EditorPrefs.SetInt("CustomAssetImport.MaxTextureSize", selectedMaxTextureSizeIndex);
            EditorPrefs.SetBool("CustomAssetImport.StreamingMipmaps", streamingMipmaps);
            EditorPrefs.SetBool("CustomAssetImport.CrunchedCompression", crunchedCompression);
            EditorPrefs.SetInt("CustomAssetImport.CompressionQuality", selectedCompressionQualityIndex);

            Debug.Log("Custom Asset Import Settings Saved");
        }
    }

    public class CustomAssetImportProcessor : AssetPostprocessor
    {
        // モデルがインポートされたときの処理
        void OnPostprocessModel(GameObject g)
        {
            if (!EditorPrefs.GetBool(CustomAssetImportWindow.PREF_KEY_ENABLE_MODEL, false)) return; // 有効でなければ何もしない

            ModelImporter modelImporter = (ModelImporter)assetImporter;

            modelImporter.bakeAxisConversion = EditorPrefs.GetBool("CustomAssetImport.BakeAxisConversion", false);
            modelImporter.importBlendShapes = EditorPrefs.GetBool("CustomAssetImport.ImportBlendshapes", true);
            modelImporter.isReadable = EditorPrefs.GetBool("CustomAssetImport.IsReadable", true);
            modelImporter.weldVertices = EditorPrefs.GetBool("CustomAssetImport.WeldVertices", true);
            modelImporter.importVisibility = EditorPrefs.GetBool("CustomAssetImport.ImportVisibility", false);
            modelImporter.importCameras = EditorPrefs.GetBool("CustomAssetImport.ImportCameras", false);
            modelImporter.importLights = EditorPrefs.GetBool("CustomAssetImport.ImportLights", false);
            modelImporter.preserveHierarchy = EditorPrefs.GetBool("CustomAssetImport.PreserveHierarchy", false);
            modelImporter.sortHierarchyByName = EditorPrefs.GetBool("CustomAssetImport.SortHierarchyByName", false);

            // Force legacy blendshape normals False
            // https://forum.unity.com/threads/legacy-blend-shape-normals-missing-on-modelimporter.1166324/
            string pName = "legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes";
            PropertyInfo prop = modelImporter.GetType().GetProperty(pName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            prop.SetValue(modelImporter, EditorPrefs.GetBool("CustomAssetImport.LegacyBlendShapeNormals", false));

            // modelImporter.importNormals = (ModelImporterNormals)EditorPrefs.GetInt("CustomAssetImport.ModelImporterNormals", (int)ModelImporterNormals.Import);
            // modelImporter.importBlendShapeNormals = (ModelImporterNormals)EditorPrefs.GetInt("CustomAssetImport.BlendShapeNormals", (int)ModelImporterNormals.None);
            // modelImporter.meshCompression = (ModelImporterMeshCompression)EditorPrefs.GetInt("CustomAssetImport.MeshCompression", (int)ModelImporterMeshCompression.Off);

            // (skip) が 0 番目にいて,以降1つずつずれているので, -1 して元に戻す.
            int meshCompressionVal = EditorPrefs.GetInt("CustomAssetImport.MeshCompression") - 1;
            if (meshCompressionVal != -1)
            {
                modelImporter.meshCompression = (ModelImporterMeshCompression)meshCompressionVal;
            }
            int importerNormalsVal = EditorPrefs.GetInt("CustomAssetImport.ModelImporterNormals") - 1;
            if (importerNormalsVal != -1)
            {
                modelImporter.importNormals = (ModelImporterNormals)importerNormalsVal;
            }
            int importBlendShapeNormalsVal = EditorPrefs.GetInt("CustomAssetImport.BlendShapeNormals") - 1;
            if (importBlendShapeNormalsVal != -1)
            {
                modelImporter.importBlendShapeNormals = (ModelImporterNormals)importBlendShapeNormalsVal;
            }
        }

        // テクスチャがインポートされたときの処理
        void OnPreprocessTexture()
        {
            if (!EditorPrefs.GetBool(CustomAssetImportWindow.PREF_KEY_ENABLE_TEX, false)) return; // 有効でなければ何もしない

            TextureImporter textureImporter = (TextureImporter)assetImporter;

            // textureImporter.maxTextureSize = 512; // テクスチャのMaxSizeを512に設定
            // int maxTextureSize = int.Parse(CustomAssetImportWindow.maxTextureSizeOptions[EditorPrefs.GetInt("CustomAssetImport.MaxTextureSize")]);
            // if (maxTextureSize < textureImporter.maxTextureSize)
            // {
            //     textureImporter.maxTextureSize = maxTextureSize;
            // }

            textureImporter.streamingMipmaps = EditorPrefs.GetBool("CustomAssetImport.StreamingMipmaps", true);
            textureImporter.crunchedCompression = EditorPrefs.GetBool("CustomAssetImport.CrunchedCompression", false);
            if (textureImporter.crunchedCompression)
            {
                textureImporter.compressionQuality = EditorPrefs.GetInt("CustomAssetImport.CompressionQuality");
            }
        }

    }
}