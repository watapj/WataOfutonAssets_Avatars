# WataOfutonAssets_Avatars

アバター用の Project 向けに作ったものをまとめています

Booth (https://wata-ofuton.booth.pm/) にて配布しているものも含まれます

何かありましたら Twitter (https://twitter.com/wata_pj) へ

## 前提環境
動作確認環境
- Unity2022.3.6f1
- Unity2019.4.31f1

以下のツールを事前に Project へインポートしておいてください
- VRChatSDK
- Modular Avatar(https://github.com/bdunderscore/modular-avatar)
- lilToon(https://lilxyzw.github.io/lilToon/#/)


## 配布物一覧
[Clip Costume Shaders(lilToon)](https://github.com/watapj/WataOfutonAssets_Avatars/raw/main/UnityPackages/lilToon_ClipCostume.unitypackage)

    lilToonの改変バージョン

[SleepSphere](https://github.com/watapj/WataOfutonAssets_Avatars/raw/main/UnityPackages/SleepSphere_v1.1.unitypackage)

[RainbowPolyRing](https://github.com/watapj/WataOfutonAssets_Avatars/raw/main/UnityPackages/RainbowPolyRing.unitypackage)


### アバター改変用エディタ拡張Pack

詳細な説明書を作成中...
WataOfuton フォルダ内に置いてあります

- TransformExpansion
 - SymmetryBoneEditor (左右対称のオブジェクトに Transform の変更を自動で適用)
 - IgnoreChildTransform (子のオブジェクトに対する Transform の変更を無視)
- MMDSetup (半自動でMMDワールドへ対応 MA対応)
- UnusedPropertiesRemover (マテリアル内の未使用パラメータを削除する)
- BoneCopier (2つのオブジェクト内にある同名オブジェクトを一覧化し、各 Transform の値をコピーする)
