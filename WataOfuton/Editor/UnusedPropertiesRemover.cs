/*
 * This code was generated with the help of ChatGPT, an AI language model developed by OpenAI.
 * Please save this script in a folder named "Editor".
 * NOTE : By adding this script, an extension "Symmetry Bone Editor" will be displayed in the Transform component of all GameObjects.
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace WataOfuton.Tool
{
    public class UnusedPropertiesRemover : MonoBehaviour
    {
        // メニューアイテムを追加
        [MenuItem("Assets/WataOfuton/Remove Unused Properties", false, 21)]
        public static void RemoveUnusedProperties()
        {
            // 選択したすべてのマテリアルを取得
            Object[] mats = Selection.GetFiltered(typeof(Material), SelectionMode.Assets);

            // すべてのマテリアルに対して
            foreach (Material mat in mats)
            {
                // マテリアルのパスを取得
                string path = AssetDatabase.GetAssetPath(mat);

                // 同じシェーダーを持つ新しいマテリアルを作成
                var newMat = new Material(mat.shader);

                // パラメータをコピー
                newMat.name = mat.name;
                newMat.renderQueue = (mat.shader.renderQueue == mat.renderQueue) ? -1 : mat.renderQueue;
                newMat.enableInstancing = mat.enableInstancing;
                newMat.doubleSidedGI = mat.doubleSidedGI;
                newMat.globalIlluminationFlags = mat.globalIlluminationFlags;
                newMat.hideFlags = mat.hideFlags;
                newMat.shaderKeywords = mat.shaderKeywords;

                // プロパティをコピー
                var properties = MaterialEditor.GetMaterialProperties(new Material[] { mat });
                foreach (var prop in properties)
                {
                    if (prop.type == MaterialProperty.PropType.Color)
                    {
                        newMat.SetColor(prop.name, mat.GetColor(prop.name));
                    }
                    else if (prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range)
                    {
                        newMat.SetFloat(prop.name, mat.GetFloat(prop.name));
                    }
                    else if (prop.type == MaterialProperty.PropType.Texture)
                    {
                        newMat.SetTexture(prop.name, mat.GetTexture(prop.name));
                    }
                    else if (prop.type == MaterialProperty.PropType.Vector)
                    {
                        newMat.SetVector(prop.name, mat.GetVector(prop.name));
                    }
                }

                // 新しいマテリアルを古いマテリアルに置き換える（GUIDを保持）
                string tempPath = path + "_temp";
                AssetDatabase.CreateAsset(newMat, tempPath);
                FileUtil.ReplaceFile(tempPath, path);
                AssetDatabase.DeleteAsset(tempPath);

                Debug.Log("未使用のプロパティがマテリアルから削除されました: " + mat.name);
            }
        }

        // 上記の関数によって定義されたメニューアイテムの検証を行う。
        // この関数がfalseを返すと、メニューアイテムは無効化されます。
        [MenuItem("Assets/WataOfuton/Remove Unused Properties", true)]
        public static bool ValidateRemoveUnusedProperties()
        {
            // マテリアルが選択されていなければfalseを返す
            return Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Length > 0;
        }
    }
}
