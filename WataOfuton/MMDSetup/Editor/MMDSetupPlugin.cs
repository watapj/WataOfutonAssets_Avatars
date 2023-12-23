using nadena.dev.ndmf;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;

[assembly: ExportsPlugin(typeof(WataOfuton.Tool.MMDSetup.Editor.MMDSetupPlugin))]

namespace WataOfuton.Tool.MMDSetup.Editor
{
    public class MMDSetupPlugin : Plugin<MMDSetupPlugin>
    {
        public override string DisplayName => nameof(MMDSetup);

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming).AfterPlugin("nadena.dev.modular-avatar").Run(nameof(MMDSetup), ctx =>
            {
                var MMDSetup = ctx.AvatarRootObject.GetComponentInChildren<MMDSetup>();
                if (MMDSetup == null) return;

                var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();
                if (descriptor.baseAnimationLayers == null) return;

                var fx = descriptor.baseAnimationLayers[4].animatorController;

                AnimatorController controller = fx as AnimatorController;
                if (controller == null)
                {
                    Debug.LogError("No AnimatorController found on the Animator.");
                    return;
                }

                var face = MMDSetup.faceMesh.gameObject;

                Debug.Log("Face Mesh Name is " + face.name);

                if (face.name != "Body")
                {
                    // 各アニメーションクリップのパスを置き換える
                    foreach (var layer in controller.layers)
                    {
                        foreach (var state in layer.stateMachine.states)
                        {
                            AnimationClip clip = state.state.motion as AnimationClip;
                            if (clip != null)
                            {
                                ReplacePathsInClip(clip, face.name);
                            }
                        }
                    }

                    // 先にリネームしたら Path の置換が成功しないのでここに.
                    face.name = "Body";

                    Debug.Log("Animation paths replaced.");
                }

                UnityEngine.Object.DestroyImmediate(MMDSetup);
            });
        }

        static void ReplacePathsInClip(AnimationClip clip, string faceName)
        {
            // アニメーションクリップ内の全バインディングを取得
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

            foreach (var binding in bindings)
            {
                // faceName を "Body" に置き換える
                // if (binding.path.Contains(faceName))
                // {
                string newPath = binding.path.Replace(faceName, "Body");
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                AnimationUtility.SetEditorCurve(clip, binding, null); // 古いバインディングを削除
                EditorCurveBinding newBinding = new EditorCurveBinding
                {
                    path = newPath,
                    propertyName = binding.propertyName,
                    type = binding.type
                };
                AnimationUtility.SetEditorCurve(clip, newBinding, curve); // 新しいバインディングに追加
                // }
            }
        }
    }
}
